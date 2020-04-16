using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using PhobiaX.Assets;
using PhobiaX.SDL2;

namespace PhobiaX.Assets
{
    public class AssetProvider : IDisposable
    {
        private readonly SDLSurfaceFactory surfaceFactory;
        private SurfaceAssets surfaces = new SurfaceAssets();
        private IDictionary<string, AnimatedCollection> animations = new Dictionary<string, AnimatedCollection>();

        public AssetProvider(SDLSurfaceFactory surfaceFactory)
        {
            this.surfaceFactory = surfaceFactory ?? throw new ArgumentNullException(nameof(surfaceFactory));
        }

        public void Dispose()
        {
            surfaces.Dispose();

            foreach (var animation in animations)
            {
                animation.Value.Dispose();
            }
        }

        public void LoadAnimations(string resourcesPath, string defaultSetName, string finalSetName, bool isFinalSetAnimation, params SDLColor[] transparencyColors)
        {
            var map = new Dictionary<string, List<string>>();
            foreach (var file in Directory.EnumerateFiles(resourcesPath, "*.bmp", SearchOption.AllDirectories))
            {
                var directoryPath = Path.GetDirectoryName(file);

                if (map.TryGetValue(directoryPath, out List<string> files))
                {
                    files.Add(file);
                }
                else
                {
                    map.Add(directoryPath, new List<string> { file });
                }
            }

            foreach (var animationSet in map)
            {
                animationSet.Value.Sort();
                var set = animationSet.Value;

                var intItems = set.Where(i => Int32.TryParse(Path.GetFileNameWithoutExtension(i), out int k));

                if (intItems.ToList().Count == animationSet.Value.Count)
                {
                    set = animationSet.Value.OrderBy(x => Int32.Parse(Path.GetFileNameWithoutExtension(x))).ToList();
                }

                var surfaces = new List<SDLSurface>();

                foreach (var animation in set)
                {
                    var surface = surfaceFactory.LoadSurface(animation);

                    SDLSurface filteredSurface = FilterOutTransparencyColors(transparencyColors, surface);

                    filteredSurface.SetColorKey(SDLColor.Black);
                    surfaces.Add(filteredSurface);
                }

                var rootFolderName = Path.GetFileName(resourcesPath).ToLower();
                var parentFolderName = Path.GetFileName(Directory.GetParent(animationSet.Key).FullName).ToLower();
                var folderName = Path.GetFileName(animationSet.Key).ToLower();

                if (animations.TryGetValue(parentFolderName, out var animatedAssets))
                {
                    animatedAssets.AddAnimation(folderName, surfaces);
                }
                else
                {
                    var animatedAsset = new AnimatedCollection(parentFolderName, defaultSetName, finalSetName, isFinalSetAnimation);
                    animatedAsset.AddAnimation(folderName, surfaces);
                    animations.Add(parentFolderName, animatedAsset);
                }
            }
        }

        private SDLSurface FilterOutTransparencyColors(SDLColor[] transparencyColors, SDLSurface surface)
        {
            var filteredSurface = surface;

            foreach (var transparencyColor in transparencyColors)
            {
                var finalSurface = surfaceFactory.CreateSurface(filteredSurface.Surface.w, filteredSurface.Surface.h);
                filteredSurface.SetColorKey(transparencyColor);
                filteredSurface.BlitSurface(finalSurface);
                filteredSurface.Dispose();

                filteredSurface = finalSurface;
            }

            return filteredSurface;
        }

        public void LoadSurfaces(string resourcesPath, params SDLColor[] transparencyColors)
        {
            foreach (var file in Directory.EnumerateFiles(resourcesPath, "*.bmp", SearchOption.AllDirectories))
            {
                var directoryName = Path.GetFileName(Path.GetDirectoryName(file));
                var fileName = Path.GetFileNameWithoutExtension(file);
                var surface = surfaceFactory.LoadSurface(file);

                var filteredSurface = FilterOutTransparencyColors(transparencyColors, surface);

                filteredSurface.SetColorKey(SDLColor.Black);

                surfaces.AddTexture($"{directoryName.ToLower()}_{fileName.ToLower()}", filteredSurface);
            }
        }

        public SurfaceAssets GetSurfaces()
        {
            return surfaces;
        }

        public IDictionary<string, AnimatedCollection> GetAnimatedSurfaces()
        {
            return animations;
        }
    }
}

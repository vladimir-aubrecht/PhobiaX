using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using PhobiaX.Assets;
using PhobiaX.SDL2;

namespace PhobiaX
{
    public class AssetProvider : IDisposable
    {
        private readonly SDLRenderer renderer;
        private SurfaceAssets surfaces = new SurfaceAssets();
        private IDictionary<string, AnimatedSet> animations = new Dictionary<string, AnimatedSet>();

        public AssetProvider(SDLRenderer renderer)
        {
            this.renderer = renderer ?? throw new ArgumentNullException(nameof(renderer));
        }

        public void Dispose()
        {
            surfaces.Dispose();

            foreach (var animation in animations)
            {
                animation.Value.Dispose();
            }
        }

        public void LoadAnimations(string resourcesPath, string defaultSetName, byte r, byte g, byte b)
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
                    var surface = renderer.LoadSurface(animation);
                    surface.SetColorKey(r, g, b);
                    surfaces.Add(surface);
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
                    var animatedAsset = new AnimatedSet(parentFolderName, defaultSetName);
                    animatedAsset.AddAnimation(folderName, surfaces);
                    animations.Add(parentFolderName, animatedAsset);
                }
            }
        }

        public void LoadSurfaces(string resourcesPath)
        {
            foreach (var file in Directory.EnumerateFiles(resourcesPath, "*.bmp", SearchOption.AllDirectories))
            {
                var directoryName = Path.GetFileName(Path.GetDirectoryName(file));
                var fileName = Path.GetFileNameWithoutExtension(file);
                var surface = renderer.LoadSurface(file);

                surfaces.AddTexture($"{directoryName.ToLower()}_{fileName.ToLower()}", surface);
            }
        }

        public SurfaceAssets GetSurfaces()
        {
            return surfaces;
        }

        public IDictionary<string, AnimatedSet> GetAnimatedSurfaces()
        {
            return animations;
        }
    }
}

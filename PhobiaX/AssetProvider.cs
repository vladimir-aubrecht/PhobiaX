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
        private AnimatedSurfaceAssets animations = new AnimatedSurfaceAssets();

        public AssetProvider(SDLRenderer renderer)
        {
            this.renderer = renderer ?? throw new ArgumentNullException(nameof(renderer));
        }

        public void Dispose()
        {
            surfaces.Dispose();
            animations.Dispose();
        }

        public void LoadAssets(string resourcesPath)
        {
            LoadSurfaces(resourcesPath);
            LoadAnimations(resourcesPath);
        }

        private void LoadAnimations(string resourcesPath)
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
                    surfaces.Add(renderer.LoadSurface(animation));
                }

                var rootFolderName = Path.GetFileName(resourcesPath);
                var parentFolderName = Path.GetFileName(Directory.GetParent(animationSet.Key).FullName);
                var folderName = Path.GetFileName(animationSet.Key);

                if (rootFolderName != parentFolderName)
                {
                    animations.AddAnimation($"{parentFolderName.ToLower()}_{folderName.ToLower()}", surfaces);
                }
                else
                {
                    animations.AddAnimation(folderName.ToLower(), surfaces);
                }

            }
        }

        private void LoadSurfaces(string resourcesPath)
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

        public AnimatedSurfaceAssets GetAnimatedSurfaces()
        {
            return animations;
        }
    }
}

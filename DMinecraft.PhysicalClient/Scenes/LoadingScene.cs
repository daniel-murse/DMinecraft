using DMinecraft.PhysicalClient.Scheduling.Coroutines;
using OpenTK.Mathematics;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMinecraft.PhysicalClient.Scenes
{
    internal class LoadingScene : IScene
    {
        private IEnumerator<ITimedCoroutine> loadingCoroutines;

        private ITimedCoroutine currentLoadingCoroutine;

        public void OnClientSizeChanged(Vector2i newSize)
        {
            throw new NotImplementedException();
        }

        public void Render(TimeSpan deltaTime)
        {
            throw new NotImplementedException();
        }

        public void Update(TimeSpan deltaTime)
        {
        }
    }
}

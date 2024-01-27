using DMinecraft.PhysicalClient.Scheduling.Coroutines;
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

        public void Render(TimeSpan deltaTime)
        {
            throw new NotImplementedException();
        }

        public void Update(TimeSpan deltaTime)
        {
        }
    }
}

using System.Threading;

namespace Scripts.Core
{
    public abstract class CustomTask
    {
        protected TaskCompleteDelegate TaskCompleteDelegate;
        
        public virtual void StartTask(CancellationTokenSource cancellationTokenSource, TaskCompleteDelegate taskCompleteDelegate)
        {
            TaskCompleteDelegate = taskCompleteDelegate;
        }

        public virtual void CompleteTask(float delaySeconds)
        {
            TaskCompleteDelegate?.Invoke(delaySeconds);
        }
    }
}
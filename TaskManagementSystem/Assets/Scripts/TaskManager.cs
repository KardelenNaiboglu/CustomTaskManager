using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace Scripts.Core
{
    public delegate void TaskCompleteDelegate(float delay);
    
    public class TaskManager
    {
        private readonly List<CustomTask> _queuedTasks = new();
        private bool _hasRunningTask = false;
        private CancellationTokenSource _cancellationToken;
        private CustomTask _currentRunningTask = null;

        public void Init()
        {
            if (_cancellationToken != null)
            {
                _cancellationToken.Cancel();
                _cancellationToken.Dispose();
                _cancellationToken = null;
            }
            
            _cancellationToken = new CancellationTokenSource();
        }
        
        public void AddTaskToQueue(CustomTask customTask)
        {
            if(customTask == null) return;
            
            _queuedTasks.Add(customTask);

            CheckTaskQueue();
        }

        private void CheckTaskQueue()
        {
            if(_queuedTasks.Count <= 0) return;
            if(_hasRunningTask) return;

            var task = _queuedTasks[0];
            _queuedTasks.RemoveAt(0);
            
            ProcessGivenTask(task);
        }

        private void ProcessGivenTask(CustomTask customTask)
        {
            _currentRunningTask = customTask;
            _hasRunningTask = true;
            customTask.StartTask(_cancellationToken, TaskCompleteAsync);
        }
        
        private async void TaskCompleteAsync(float delaySeconds)
        {
            if (delaySeconds > 0)
            {
                await Task.Delay(Mathf.RoundToInt(delaySeconds * 1000));
            }
            
            _currentRunningTask = null;
            _hasRunningTask = false;
            CheckTaskQueue();
        }
        
    }
}
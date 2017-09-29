using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

namespace Pathfinding
{
    //This class controls the threads
    public class PathfindMaster : SingleGameObject<PathfindMaster>
	{
        //The maximum simultaneous threads we allow to open
        public int MaxJobs = 2;

        //Delegates are a variable that points to a function
        public delegate void PathfindingJobComplete(List<Node> path);

        private List<Pathfinder> currentJobs;
        private List<Pathfinder> todoJobs;

        protected override void Awake()
        {
            //base.Awake();

            //currentJobs = new List<Pathfinder>();
            //todoJobs = new List<Pathfinder>();
            //Update();
        }

        //void _Update() 
        //{
        //    /*
        //     * Another way to keep track of the threads we have open would have been to create 
        //     * a new thread for it but we can also just use Unity's main thread too since this class
        //     * derives from monoBehaviour
        //     */

        //    int i = 0;

        //    while(i < currentJobs.Count)
        //    {
        //        if(currentJobs[i].jobDone)
        //        {
        //            currentJobs[i].NotifyComplete();
        //            currentJobs.RemoveAt(i);
        //        }
        //        else
        //        {
        //            i++;
        //        }
        //    }

        //    if(todoJobs.Count > 0 && currentJobs.Count < MaxJobs)
        //    {
        //        Pathfinder job = todoJobs[0];
        //        todoJobs.RemoveAt(0);
        //        currentJobs.Add(job);

        //        //Start a new thread

        //        Thread jobThread = new Thread(job.FindPath);
        //        jobThread.Start();

        //        //As per the doc
        //        //https://msdn.microsoft.com/en-us/library/system.threading.thread(v=vs.110).aspx
        //        //It is not necessary to retain a reference to a Thread object once you have started the thread. 
        //        //The thread continues to execute until the thread procedure is complete.				
        //    }
        //}

        public IEnumerator RequestPathfind(Node start, Node target, PathfindingJobComplete completeCallback, IGetNode nodeManager)
        {
			Pathfinder newJob = new Pathfinder(start, target, completeCallback, nodeManager);
            newJob.FindPath();

            yield return new WaitUntil(()=> newJob.jobDone);

            newJob.NotifyComplete();
        }

        public List<Node> RequestPathfind(Node start, Node target, IGetNode nodeManager)
        {
            Pathfinder newJob = new Pathfinder(start, target, null, nodeManager);

            return newJob.FindPath();
        }
    }
}

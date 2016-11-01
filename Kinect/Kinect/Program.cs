using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Kinect;

namespace Kinect
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            // Find the first connected sensor
            KinectSensor sensor = KinectSensor.KinectSensors.Where(s => s.Status == KinectStatus.Connected).FirstOrDefault();
            if (sensor == null)
            {
                Console.WriteLine("No Kinect sensor found!");
                return;
            }

            // Create object that will track skeletons using the sensor
            Tracker tracker = new Tracker(sensor);

            // Start the sensor
            sensor.Start();

            // Run until the user presses 'q' or 'Q' on the keyboard
            while (Char.ToLowerInvariant(Console.ReadKey().KeyChar) != 'q') { }

            // Stop the sensor
            sensor.Stop();
        }
    }

    internal class Tracker
    {
        int frame = 0;
        DateTime time = DateTime.Now;
        //double minutes = time.Minute;

        //Gait Parameters
        double cadence = 0;
        double stepLength = 0;
        double stepTime = 0;
        double stepWidth = 0;
        double stanceTime = 0;
        double strideLength = 0;
        double strideVelocity = 0;
        double swingTime = 0;

        double totalDistance = 0;

        double[] initialPoint = new double[3];
        double[] currentPoint = new double[3];

        private Skeleton[] skeletons = null;

        public Tracker(KinectSensor sensor)
        {
            // Connect the skeleton frame handler and enable skeleton tracking
            sensor.SkeletonFrameReady += SensorSkeletonFrameReady;
            sensor.SkeletonStream.Enable();
            sensor.DepthStream.Enable();
        }

        public object FloorClipPane { get; private set; }

        private void SensorSkeletonFrameReady(object sender, SkeletonFrameReadyEventArgs e)
        {
            // Access the skeleton frame
            using (SkeletonFrame skeletonFrame = e.OpenSkeletonFrame())
            {
                if (skeletonFrame != null)
                {
                    if (this.skeletons == null)
                    {
                        // Allocate array of skeletons
                        this.skeletons = new Skeleton[skeletonFrame.SkeletonArrayLength];
                    }

                    // Copy skeletons from this frame
                    skeletonFrame.CopySkeletonDataTo(this.skeletons);

                    // Find first tracked skeleton, if any
                    Skeleton skeleton = this.skeletons.Where(s => s.TrackingState == SkeletonTrackingState.Tracked).FirstOrDefault();

                    if (skeleton != null)
                    {
                        // Obtain parameters; if tracked, print its position
                        Joint kneeLeft = skeleton.Joints[JointType.KneeLeft];
                        Joint kneeRight = skeleton.Joints[JointType.KneeRight];
                        Joint ankleLeft = skeleton.Joints[JointType.AnkleLeft];
                        Joint ankleRight = skeleton.Joints[JointType.AnkleRight];
                        Joint footLeft = skeleton.Joints[JointType.FootLeft];
                        Joint footRight = skeleton.Joints[JointType.FootRight];
                        //Joint hipLeft = skeleton.Joints[JointType.HipLeft];
                        //Joint hipRight = skeleton.Joints[JointType.HipRight];
                        //Joint hipCenter = skeleton.Joints[JointType.HipCenter];

                        //Step Length
                        if (ankleLeft.TrackingState == JointTrackingState.Tracked || ankleLeft.TrackingState == JointTrackingState.Tracked)
                        {
                            stepLength = Math.Round(Math.Sqrt(Math.Pow(ankleRight.Position.X - ankleLeft.Position.X, 2) +
                                        Math.Pow(ankleRight.Position.Y - ankleLeft.Position.Y, 2) +
                                        Math.Pow(ankleRight.Position.Z - ankleLeft.Position.Z, 2)) * 100, 2);
                        }

                        //Distance Traveled
                        if (footLeft.TrackingState == JointTrackingState.Tracked)
                        {
                            if(initialPoint[0] == 0 && initialPoint[1] == 0 && initialPoint[2] == 0)
                            {
                                initialPoint[0] = footLeft.Position.X;
                                initialPoint[1] = footLeft.Position.Y;
                                initialPoint[2] = footLeft.Position.Z;
                            }
                            else
                            {
                                currentPoint[0] = footLeft.Position.X;
                                currentPoint[1] = footLeft.Position.Y;
                                currentPoint[2] = footLeft.Position.Z;
                                totalDistance = Math.Round(Math.Sqrt(Math.Pow(initialPoint[0] - currentPoint[0], 2) +
                                                Math.Pow(initialPoint[0] - currentPoint[0], 2) +
                                                Math.Pow(initialPoint[0] - currentPoint[0], 2)) * 100, 2);
                            }
                        }

                        Console.Clear();
                        Console.WriteLine("Frame: " + ++frame);
                        Console.WriteLine("Step Length: " + stepLength + "cm");
                        Console.WriteLine("Distance Traveled: " + totalDistance + "cm");
                        Console.WriteLine(time.Hour + ":" + time.Minute + ":" + time.Second);
                        Console.WriteLine();
                    }
                }
            }
        }
    }
}

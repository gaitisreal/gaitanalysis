﻿using System;
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
<<<<<<< HEAD
        int frame = 0;
        DateTime startTime;
        DateTime time;
        DateTime terminateTime;

        bool testStart = false;

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
        double totalTime = 0;

        double initialTime = 0;
        double currentTime = 0;

        double[] initialPoint = new double[3];
        double[] currentPoint = new double[3];

        Tuple<float, float, float, float> floorPlane;

=======
>>>>>>> refs/remotes/origin/master
        private Skeleton[] skeletons = null;

        public Tracker(KinectSensor sensor)
        {
            // Connect the skeleton frame handler and enable skeleton tracking
            sensor.SkeletonFrameReady += SensorSkeletonFrameReady;
            sensor.SkeletonStream.Enable();
<<<<<<< HEAD
            sensor.DepthStream.Enable();
        }

        public Tuple<float, float, float, float> FloorClipPlane { get; set; }

        private void SensorSkeletonFrameReady(object sender, SkeletonFrameReadyEventArgs e)
        {
            floorPlane = FloorClipPlane;
=======
        }

        private void SensorSkeletonFrameReady(object sender, SkeletonFrameReadyEventArgs e)
        {
            int ctr = 0;
            double cm = 0;
>>>>>>> refs/remotes/origin/master
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
<<<<<<< HEAD
                    
=======

>>>>>>> refs/remotes/origin/master
                    // Copy skeletons from this frame
                    skeletonFrame.CopySkeletonDataTo(this.skeletons);

                    // Find first tracked skeleton, if any
                    Skeleton skeleton = this.skeletons.Where(s => s.TrackingState == SkeletonTrackingState.Tracked).FirstOrDefault();

                    if (skeleton != null)
                    {
<<<<<<< HEAD
                        // Obtain parameters; if tracked, print its position
=======
                        // Obtain the left knee joint; if tracked, print its position
>>>>>>> refs/remotes/origin/master
                        Joint kneeLeft = skeleton.Joints[JointType.KneeLeft];
                        Joint kneeRight = skeleton.Joints[JointType.KneeRight];
                        Joint ankleLeft = skeleton.Joints[JointType.AnkleLeft];
                        Joint ankleRight = skeleton.Joints[JointType.AnkleRight];
                        Joint footLeft = skeleton.Joints[JointType.FootLeft];
                        Joint footRight = skeleton.Joints[JointType.FootRight];
<<<<<<< HEAD
                        //Joint hipLeft = skeleton.Joints[JointType.HipLeft];
                        //Joint hipRight = skeleton.Joints[JointType.HipRight];
                        //Joint hipCenter = skeleton.Joints[JointType.HipCenter];

                        time = DateTime.Now;

                        //Step Length
                        if (ankleLeft.TrackingState == JointTrackingState.Tracked || ankleLeft.TrackingState == JointTrackingState.Tracked)
                        {
                            stepLength = Math.Round(Math.Sqrt(Math.Pow(ankleRight.Position.X - ankleLeft.Position.X, 2) +
                                        Math.Pow(ankleRight.Position.Y - ankleLeft.Position.Y, 2) +
                                        Math.Pow(ankleRight.Position.Z - ankleLeft.Position.Z, 2)) * 100, 2);
                        }

                        //Stride Velocity
                        if (footLeft.TrackingState == JointTrackingState.Tracked)
                        {
                            if (initialPoint[0] == 0 && initialPoint[1] == 0 && initialPoint[2] == 0)
                            {
                                initialPoint[0] = footLeft.Position.X;
                                initialPoint[1] = footLeft.Position.Y;
                                initialPoint[2] = footLeft.Position.Z;
                                initialTime = time.Hour * 3600 + time.Minute * 60 + time.Second;
                                startTime = DateTime.Now;
                                testStart = !testStart;
                            }
                            else
                            {
                                currentPoint[0] = footLeft.Position.X;
                                currentPoint[1] = footLeft.Position.Y;
                                currentPoint[2] = footLeft.Position.Z;
                                currentTime = time.Hour * 3600 + time.Minute * 60 + time.Second;

                                totalTime = currentTime - initialTime;
                                totalDistance = Math.Round(Math.Sqrt(Math.Pow(initialPoint[0] - currentPoint[0], 2) +
                                                Math.Pow(initialPoint[0] - currentPoint[0], 2) +
                                                Math.Pow(initialPoint[0] - currentPoint[0], 2)) * 100, 2);
                                strideVelocity = Math.Round(totalDistance / totalTime, 2);
                            }
                        }

                        if(frame%10 == 9)
                            Console.Clear();

                        Console.WriteLine("Frame: " + ++frame);
                        Console.WriteLine();
                        Console.WriteLine("Gait Parameters");
                        Console.WriteLine("Step Length: " + stepLength + "cm");
                        Console.WriteLine("Stride Velocity: " + strideVelocity + "cm/s");
                        Console.WriteLine();
                        Console.WriteLine("Distance Traveled: " + totalDistance + "cm");
                        Console.WriteLine("Time Traveled: " + totalTime + "s");
                        Console.WriteLine();
                        Console.WriteLine("Floor Plane: " + floorPlane.Item1 + " " + floorPlane.Item2 + " " + floorPlane.Item3 + " " + floorPlane.Item4);
                        Console.WriteLine("Start Time: " + startTime.Hour + ":" + startTime.Minute + ":" + startTime.Second);
                        Console.WriteLine("Current Time: " + time.Hour + ":" + time.Minute + ":" + time.Second);
                        Console.WriteLine();
                    }

                    /*else if(testStart)
                    {
                        terminateTime = DateTime.Now;
                        if(time.Hour * 3600 + time.Minute * 60 + time.Second + 5 == terminateTime.Hour * 3600 + terminateTime.Minute * 60 + terminateTime.Second)
                        {
                            System.exit;
                        }
                    }*/
=======
                        Joint hipLeft = skeleton.Joints[JointType.HipLeft];
                        Joint hipRight = skeleton.Joints[JointType.HipRight];
                        Joint hipCenter = skeleton.Joints[JointType.HipCenter];

                        //Console.WriteLine(ankleLeft.Position.X + " " + ankleLeft.Position.Y + " " + ankleLeft.Position.Z);

                        double distance = Math.Sqrt(Math.Pow(skeleton.Joints[JointType.HandRight].Position.X - skeleton.Joints[JointType.HandLeft].Position.X, 2) + Math.Pow(skeleton.Joints[JointType.HandRight].Position.Y - skeleton.Joints[JointType.HandLeft].Position.Y, 2) +
                            Math.Pow(skeleton.Joints[JointType.HandRight].Position.Z - skeleton.Joints[JointType.HandLeft].Position.Z, 2));
                        double stepLength = Math.Round(distance * 100, 2);

                        /*if ((ankleLeft.Position.Y > -0.54 && ankleLeft.Position.Y < -0.53 && ankleRight.Position.Y > -0.54 && ankleRight.Position.Y < -0.53) &&
                            (ankleLeft.Position.Y > 1.79 && ankleLeft.Position.Y < 1.80 && ankleRight.Position.Y > 1.79 && ankleRight.Position.Y < 1.80))*/
                        //Console.Clear();
                        Console.WriteLine(stepLength);
                    }
>>>>>>> refs/remotes/origin/master
                }
            }
        }
    }
<<<<<<< HEAD
}
=======
}
>>>>>>> refs/remotes/origin/master

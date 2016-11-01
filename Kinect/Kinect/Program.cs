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
        DateTime startTime;
        DateTime time;

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

        double strideLengthTotal = 0;
        double stepLengthTotal = 0;

        double[] stepInitialPoint = new double[3];
        double[] stepTerminalPoint = new double[3];

        bool leftOnFloor = false;
        bool rightOnFloor = false;
        bool stance = false;

        private Skeleton[] skeletons = null;
        Tuple<float, float, float, float> floorPlane;

        public Tracker(KinectSensor sensor)
        {
            // Connect the skeleton frame handler and enable skeleton tracking
            sensor.SkeletonFrameReady += SensorSkeletonFrameReady;
            sensor.SkeletonStream.Enable();
            sensor.DepthStream.Enable();
        }

        private void SensorSkeletonFrameReady(object sender, SkeletonFrameReadyEventArgs e)
        {
            // Access the skeleton frame
            using (SkeletonFrame skeletonFrame = e.OpenSkeletonFrame())
            {
                floorPlane = skeletonFrame.FloorClipPlane;
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

                    floorPlane = skeletonFrame.FloorClipPlane;

                    if (skeleton != null)
                    {
                        // Obtain parameters; if tracked, print its position
                        Joint kneeLeft = skeleton.Joints[JointType.KneeLeft];
                        Joint kneeRight = skeleton.Joints[JointType.KneeRight];
                        Joint ankleLeft = skeleton.Joints[JointType.AnkleLeft];
                        Joint ankleRight = skeleton.Joints[JointType.AnkleRight];
                        Joint footLeft = skeleton.Joints[JointType.FootLeft];
                        Joint footRight = skeleton.Joints[JointType.FootRight];

                        time = DateTime.Now;

                        double leftFootFloorDistance = Math.Round(Math.Abs(((double)floorPlane.Item1 * footLeft.Position.X) + ((double)floorPlane.Item2 * footLeft.Position.Y) + ((double)floorPlane.Item3 * footLeft.Position.Z) + (double)floorPlane.Item4) /
                                   Math.Sqrt(Math.Pow(footLeft.Position.X, 2) + Math.Pow(footLeft.Position.Y, 2) + Math.Pow(footLeft.Position.Z, 2)) * 1000, 2);

                        double rightFootFloorDistance = Math.Round(Math.Abs(((double)floorPlane.Item1 * footRight.Position.X) + ((double)floorPlane.Item2 * footRight.Position.Y) + ((double)floorPlane.Item3 * footRight.Position.Z) + (double)floorPlane.Item4) /
                                   Math.Sqrt(Math.Pow(footRight.Position.X, 2) + Math.Pow(footRight.Position.Y, 2) + Math.Pow(footRight.Position.Z, 2)) * 1000, 2);

                        leftOnFloor = leftFootFloorDistance < 25;
                        rightOnFloor = rightFootFloorDistance < 25;

                        stance = leftOnFloor && rightOnFloor;

                        if (stance)
                        {
                            //Console.WriteLine("Stance");
                            Console.Clear();
                            Console.BackgroundColor = ConsoleColor.Red;
                        }
                        else
                        {
                            //Console.WriteLine("Swing");
                            Console.Clear();
                            Console.BackgroundColor = ConsoleColor.Blue;
                        }
                            

                        /*if (stance == (leftOnFloor && rightOnFloor))
                        {
                            Console.WriteLine("Swing");
                            stance = leftOnFloor && rightOnFloor;
                        }
                        else
                        {
                            Console.WriteLine("Stance");
                            stance = !(leftOnFloor && rightOnFloor);
                        }*/

                            //Stride Length
                        if (ankleLeft.TrackingState == JointTrackingState.Tracked || ankleLeft.TrackingState == JointTrackingState.Tracked)
                        {
                            strideLengthTotal += Math.Round(Math.Sqrt(Math.Pow(ankleRight.Position.X - ankleLeft.Position.X, 2) +
                                        Math.Pow(ankleRight.Position.Y - ankleLeft.Position.Y, 2) +
                                        Math.Pow(ankleRight.Position.Z - ankleLeft.Position.Z, 2)) * 100, 2);

                            strideLength = Math.Round(strideLengthTotal / frame, 2);
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

                        //Step Length
                        if (!leftOnFloor)
                        {
                            if (rightOnFloor)
                            {
                                if (stepInitialPoint[0] == 0 && stepInitialPoint[1] == 0 && stepInitialPoint[2] == 0)
                                {
                                    stepInitialPoint[0] = footLeft.Position.X;
                                    stepInitialPoint[1] = footLeft.Position.Y;
                                    stepInitialPoint[2] = footLeft.Position.Z;
                                }
                                else
                                {
                                    stepTerminalPoint[0] = footLeft.Position.X;
                                    stepTerminalPoint[1] = footLeft.Position.Y;
                                    stepTerminalPoint[2] = footLeft.Position.Z;
                                }
                            }
                            else
                            {
                                if (!(stepInitialPoint[0] == 0 && stepInitialPoint[1] == 0 && stepInitialPoint[2] == 0))
                                {
                                    stepLengthTotal += Math.Round(Math.Sqrt(Math.Pow(stepInitialPoint[0] - stepTerminalPoint[0], 2) +
                                                Math.Pow(stepInitialPoint[0] - stepTerminalPoint[0], 2) +
                                                Math.Pow(stepInitialPoint[0] - stepTerminalPoint[0], 2)) * 100, 2);
                                    stepLength = Math.Round(stepLengthTotal / frame, 2);

                                    stepInitialPoint[0] = 0;
                                    stepInitialPoint[1] = 0;
                                    stepInitialPoint[2] = 0;

                                    stepTerminalPoint[0] = 0;
                                    stepTerminalPoint[1] = 0;
                                    stepTerminalPoint[2] = 0;
                                }
                            }
                        }
                        /*if (frame % 30 == 9)
                            Console.Clear();

                        Console.WriteLine("Frame: " + ++frame);
                        Console.WriteLine();
                        Console.WriteLine("Gait Parameters");
                        Console.WriteLine("Step Length: " + stepLength + "cm");
                        Console.WriteLine("Stride Length: " + strideLength + "cm");
                        Console.WriteLine("Stride Velocity: " + strideVelocity + "cm/s");
                        Console.WriteLine();
                        Console.WriteLine("Left Foot distance from Floor: " + leftFootFloorDistance);
                        Console.WriteLine("Right Foot distance from Floor: " + rightFootFloorDistance);
                        Console.WriteLine("Stance Status: " + stance);
                        Console.WriteLine("Distance Traveled: " + totalDistance + "cm");
                        Console.WriteLine("Time Traveled: " + totalTime + "s");
                        Console.WriteLine();
                        Console.WriteLine("Start Time: " + startTime.Hour + ":" + startTime.Minute + ":" + startTime.Second);
                        Console.WriteLine("Current Time: " + time.Hour + ":" + time.Minute + ":" + time.Second);
                        Console.WriteLine();*/
                    }

                    /*else if(testStart)
                    {
                        terminateTime = DateTime.Now;
                        if(time.Hour * 3600 + time.Minute * 60 + time.Second + 5 == terminateTime.Hour * 3600 + terminateTime.Minute * 60 + terminateTime.Second)
                        {
                            System.exit;
                        }
                    }*/
                }
            }
        }
    }
}
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
        private Skeleton[] skeletons = null;

        public Tracker(KinectSensor sensor)
        {
            // Connect the skeleton frame handler and enable skeleton tracking
            sensor.SkeletonFrameReady += SensorSkeletonFrameReady;
            sensor.SkeletonStream.Enable();
        }

        private void SensorSkeletonFrameReady(object sender, SkeletonFrameReadyEventArgs e)
        {
            int ctr = 0;
            double cm = 0;
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
                        // Obtain the left knee joint; if tracked, print its position
                        Joint kneeLeft = skeleton.Joints[JointType.KneeLeft];
                        Joint kneeRight = skeleton.Joints[JointType.KneeRight];
                        Joint ankleLeft = skeleton.Joints[JointType.AnkleLeft];
                        Joint ankleRight = skeleton.Joints[JointType.AnkleRight];
                        Joint footLeft = skeleton.Joints[JointType.FootLeft];
                        Joint footRight = skeleton.Joints[JointType.FootRight];
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
                }
            }
        }
    }
}

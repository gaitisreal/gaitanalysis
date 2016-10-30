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

                        if (kneeLeft.TrackingState == JointTrackingState.Tracked)
                            Console.WriteLine("Left Knee: " + kneeLeft.Position.X + ", " + kneeLeft.Position.Y + ", " + kneeLeft.Position.Z);
                        if (kneeRight.TrackingState == JointTrackingState.Tracked)
                            Console.WriteLine("Right Knee: " + kneeRight.Position.X + ", " + kneeRight.Position.Y + ", " + kneeRight.Position.Z);
                        Console.WriteLine();
                        if (ankleLeft.TrackingState == JointTrackingState.Tracked)
                            Console.WriteLine("Left Ankle: " + ankleLeft.Position.X + ", " + ankleLeft.Position.Y + ", " + ankleLeft.Position.Z);
                        if (ankleRight.TrackingState == JointTrackingState.Tracked)
                            Console.WriteLine("Right Ankle: " + ankleRight.Position.X + ", " + ankleRight.Position.Y + ", " + ankleRight.Position.Z);
                        Console.WriteLine();
                        if (footLeft.TrackingState == JointTrackingState.Tracked)
                            Console.WriteLine("Left Foot: " + footLeft.Position.X + ", " + footLeft.Position.Y + ", " + footLeft.Position.Z);
                        if (footRight.TrackingState == JointTrackingState.Tracked)
                            Console.WriteLine("Right Foot: " + footRight.Position.X + ", " + footRight.Position.Y + ", " + footRight.Position.Z);
                        Console.WriteLine();
                        if (hipLeft.TrackingState == JointTrackingState.Tracked)
                            Console.WriteLine("Left Hip: " + hipLeft.Position.X + ", " + hipLeft.Position.Y + ", " + hipLeft.Position.Z);
                        if (hipRight.TrackingState == JointTrackingState.Tracked)
                            Console.WriteLine("Right Hip: " + hipRight.Position.X + ", " + hipRight.Position.Y + ", " + hipRight.Position.Z);
                        if (hipCenter.TrackingState == JointTrackingState.Tracked)
                            Console.WriteLine("Center Hip: " + hipCenter.Position.X + ", " + hipCenter.Position.Y + ", " + hipCenter.Position.Z);
                    }
                }
            }
        }
    }
    /*class Program
    {
        KinectSensor kinect = null;
        Skeleton[] skeletonData = null;

        static void Main(string[] args)
        {
            Program program = new Program();
            program.StartKinectST();
            Console.ReadLine();
        }

        void StartKinectST()
        {
            kinect = KinectSensor.KinectSensors.FirstOrDefault(s => s.Status == KinectStatus.Connected); // Get first Kinect Sensor
            kinect.SkeletonStream.Enable(); // Enable skeletal tracking

            skeletonData = new Skeleton[kinect.SkeletonStream.FrameSkeletonArrayLength]; // Allocate ST data

            kinect.SkeletonFrameReady += new EventHandler<SkeletonFrameReadyEventArgs>(kinect_SkeletonFrameReady); // Get Ready for Skeleton Ready Events

            kinect.Start(); // Start Kinect sensor
        }

        private void kinect_SkeletonFrameReady(object sender, SkeletonFrameReadyEventArgs e)
        {
            using (SkeletonFrame skeletonFrame = e.OpenSkeletonFrame()) // Open the Skeleton frame
            {
                if (skeletonFrame != null && this.skeletonData != null) // check that a frame is available
                {
                    skeletonFrame.CopySkeletonDataTo(this.skeletonData); // get the skeletal information in this frame
                }

                foreach (var skeleton in skeletonData)
                {
                    // skip the skeleton if it is not being tracked
                    if (skeleton.TrackingState != SkeletonTrackingState.Tracked)
                        continue;

                    // print the RightHand Joint position to the debug console
                    Console.WriteLine(skeleton.Joints[JointType.HandRight]);
                }
            }
        }

        private void DrawSkeletons()
        {
            foreach (Skeleton skeleton in this.skeletonData)
            {
                if (skeleton.TrackingState == SkeletonTrackingState.Tracked)
                {
                    DrawTrackedSkeletonJoints(skeleton.Joints);
                }
                else if (skeleton.TrackingState == SkeletonTrackingState.PositionOnly)
                {
                    //DrawSkeletonPosition(skeleton.Position);
                }
            }
        }

        private void DrawTrackedSkeletonJoints(JointCollection jointCollection)
        {
            // Render Head and Shoulders
            DrawBone(jointCollection[JointType.Head], jointCollection[JointType.ShoulderCenter]);
            DrawBone(jointCollection[JointType.ShoulderCenter], jointCollection[JointType.ShoulderLeft]);
            DrawBone(jointCollection[JointType.ShoulderCenter], jointCollection[JointType.ShoulderRight]);

            // Render Left Arm
            DrawBone(jointCollection[JointType.ShoulderLeft], jointCollection[JointType.ElbowLeft]);
            DrawBone(jointCollection[JointType.ElbowLeft], jointCollection[JointType.WristLeft]);
            DrawBone(jointCollection[JointType.WristLeft], jointCollection[JointType.HandLeft]);

            // Render Right Arm
            DrawBone(jointCollection[JointType.ShoulderRight], jointCollection[JointType.ElbowRight]);
            DrawBone(jointCollection[JointType.ElbowRight], jointCollection[JointType.WristRight]);
            DrawBone(jointCollection[JointType.WristRight], jointCollection[JointType.HandRight]);

            // Render other bones...
        }

        private void DrawBone(Joint jointFrom, Joint jointTo)
        {
            if (jointFrom.TrackingState == JointTrackingState.NotTracked || jointTo.TrackingState == JointTrackingState.NotTracked)
            {
                return; // nothing to draw, one of the joints is not tracked
            }

            if (jointFrom.TrackingState == JointTrackingState.Inferred || jointTo.TrackingState == JointTrackingState.Inferred)
            {
                //DrawNonTrackedBoneLine(jointFrom.Position, jointTo.Position);  // Draw thin lines if either one of the joints is inferred
            }

            if (jointFrom.TrackingState == JointTrackingState.Tracked && jointTo.TrackingState == JointTrackingState.Tracked)
            {
                //DrawTrackedBoneLine(jointFrom.Position, jointTo.Position);  // Draw bold lines if the joints are both tracked
            }
        }
    }*/
}

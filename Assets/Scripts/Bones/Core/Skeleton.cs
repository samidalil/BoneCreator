using Bones.Data;
using Bones.Math;
using System.Collections.Generic;
using UnityEngine;

namespace Bones.Core
{
    /// <summary>
    /// Represents a skeleton with multiple bones
    /// </summary>
    public sealed class Skeleton : MonoBehaviour
    {
        #region Unity Fields
        
        [SerializeField]
        [Tooltip("Epsilon used to generate the skeleton")]
        private float epsilon;

        [SerializeField]
        [Tooltip("Bones list")]
        private List<Transform> _rig = new List<Transform>();

        [SerializeField]
        [Tooltip("Usually hips bone")]
        private Transform _rootBone = null;

        #endregion

        #region Unity Callbacks

        /// <summary>
        /// Fired on draw gizmos tick
        /// </summary>
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;

            foreach (Transform bone in this._rig)
                Gizmos.DrawSphere(bone.position, 0.01f);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Initializes the skeleton and its bones
        /// </summary>
        public void Initialize(
            Vector3[] headVertices,
            Vector3[] bodyVertices,
            Vector3[] leftUpperArmVertices,
            Vector3[] leftForearmVertices,
            Vector3[] leftHandVertices,
            Vector3[] rightUpperArmVertices,
            Vector3[] rightForearmVertices,
            Vector3[] rightHandVertices,
            Vector3[] leftLegVertices,
            Vector3[] leftForelegVertices,
            Vector3[] leftFootVertices,
            Vector3[] rightLegVertices,
            Vector3[] rightForelegVertices,
            Vector3[] rightFootVertices,
            float epsilon
        )
        {
            this.epsilon = epsilon;

            Segment headPrimaryComponent = Geometry.GenerateApproximatedPrimaryComponent(headVertices);
            Segment bodyPrimaryComponent = Geometry.GenerateApproximatedPrimaryComponent(bodyVertices);
            Segment leftUpperArmPrimaryComponent = Geometry.GenerateApproximatedPrimaryComponent(leftUpperArmVertices);
            Segment leftForearmPrimaryComponent = Geometry.GenerateApproximatedPrimaryComponent(leftForearmVertices);
            Segment leftHandPrimaryComponent = Geometry.GenerateApproximatedPrimaryComponent(leftHandVertices);
            Segment rightUpperArmPrimaryComponent = Geometry.GenerateApproximatedPrimaryComponent(rightUpperArmVertices);
            Segment rightForearmPrimaryComponent = Geometry.GenerateApproximatedPrimaryComponent(rightForearmVertices);
            Segment rightHandPrimaryComponent = Geometry.GenerateApproximatedPrimaryComponent(rightHandVertices);
            Segment leftLegPrimaryComponent = Geometry.GenerateApproximatedPrimaryComponent(leftLegVertices);
            Segment leftForelegPrimaryComponent = Geometry.GenerateApproximatedPrimaryComponent(leftForelegVertices);
            Segment leftFootPrimaryComponent = Geometry.GenerateApproximatedPrimaryComponent(leftFootVertices);
            Segment rightLegPrimaryComponent = Geometry.GenerateApproximatedPrimaryComponent(rightLegVertices);
            Segment rightForelegPrimaryComponent = Geometry.GenerateApproximatedPrimaryComponent(rightForelegVertices);
            Segment rightFootPrimaryComponent = Geometry.GenerateApproximatedPrimaryComponent(rightFootVertices);

            // Hips

            Link(leftLegPrimaryComponent, leftForelegPrimaryComponent, epsilon);
            Link(rightLegPrimaryComponent, rightForelegPrimaryComponent, epsilon);

            Segment hipSegment = new Segment(leftLegPrimaryComponent.Start, rightLegPrimaryComponent.Start);
            GameObject hips = CreateJointObject(hipSegment.Center, "Hips");

            GameObject leftLeg = CreateJointObject(hipSegment.Start, "Left Leg", hips);
            GameObject leftForeleg = CreateJoint(leftLeg, leftLegPrimaryComponent, leftForelegPrimaryComponent, "Left Foreleg");
            GameObject leftFootStart = CreateJoint(leftForeleg, leftForelegPrimaryComponent, leftFootPrimaryComponent, "Left Foot Start");
            GameObject leftFootEnd = CreateJointObject(leftFootPrimaryComponent.End, "Left Foot End", leftFootStart);

            GameObject rightLeg = CreateJointObject(hipSegment.End, "Right Leg", hips);
            GameObject rightForeleg = CreateJoint(rightLeg, rightLegPrimaryComponent, rightForelegPrimaryComponent, "Right Foreleg");
            GameObject rightFootStart = CreateJoint(rightForeleg, rightForelegPrimaryComponent, rightFootPrimaryComponent, "Right Foot Start");
            GameObject rightFootEnd = CreateJointObject(rightFootPrimaryComponent.End, "Right Foot End", rightFootStart);

            // Torso

            Link(leftUpperArmPrimaryComponent, leftForearmPrimaryComponent, epsilon);
            Link(rightUpperArmPrimaryComponent, rightForearmPrimaryComponent, epsilon);

            Segment shoulderSegment = new Segment(leftUpperArmPrimaryComponent.Start, rightUpperArmPrimaryComponent.Start);
            GameObject torso = CreateJointObject(shoulderSegment.Center, "Torso", hips);

            GameObject leftArm = CreateJointObject(shoulderSegment.Start, "Left Arm", torso);
            GameObject leftForearm = CreateJoint(leftArm, leftUpperArmPrimaryComponent, leftForearmPrimaryComponent, "Left Forearm");
            GameObject leftHandStart = CreateJoint(leftForearm, leftForearmPrimaryComponent, leftHandPrimaryComponent, "Left Hand Start");
            GameObject leftHandEnd = CreateJointObject(leftHandPrimaryComponent.End, "Left Hand End", leftHandStart);

            GameObject rightArm = CreateJointObject(shoulderSegment.End, "Right Arm", torso);
            GameObject rightForearm = CreateJoint(rightArm, rightUpperArmPrimaryComponent, rightForearmPrimaryComponent, "Right Forearm");
            GameObject rightHandStart = CreateJoint(rightForearm, rightForearmPrimaryComponent, rightHandPrimaryComponent, "Right Hand Start");
            GameObject rightHandEnd = CreateJointObject(rightHandPrimaryComponent.End, "Right Hand End", rightHandStart);

            // Head

            GameObject head = CreateJoint(torso, bodyPrimaryComponent, headPrimaryComponent, "Head Start");
            GameObject headEnd = CreateJointObject(headPrimaryComponent.End, "Head End", head);

            hips.transform.SetParent(this.transform);

            this._rootBone = hips.transform;
        }

        #endregion

        #region Private Static Methods

        private GameObject CreateJoint(GameObject parentJoint, Segment parent, Segment child, string name)
        {
            return Link(parent, child, this.epsilon)
                ? CreateJointObject(parent.End, name, parentJoint)
                : CreateJointObject(child.Start, name, CreateJointObject(child.End, $"intermediate-{name}", parentJoint));
        }

        private GameObject CreateJointObject(Vector3 position, string name, GameObject parent = null)
        {
            GameObject joint = new GameObject(name);

            this._rig.Add(joint.transform);

            joint.transform.position = position;

            if (parent) joint.transform.SetParent(parent.transform);
            return joint;
        }

        /// <summary>
        /// Orders the segments' points and links the two bones' segments
        /// </summary>
        /// <param name="parent">Parent bone's segment</param>
        /// <param name="child">Child bone's segment</param>
        /// <returns>True if distance between the two bones is negligeable</returns>
        private static bool Link(Segment parent, Segment child, float epsilon)
        {
            int parentClosestPointIndex = 0;
            int childClosestPointIndex = 0;

            float minDistance = float.MaxValue;
            float distance;

            for (int i = 0; i < 2; ++i)
            {
                for (int j = 0; j < 2; ++j)
                {
                    if ((distance = SqrDistance(parent[i], child[j])) < minDistance)
                    {
                        parentClosestPointIndex = i;
                        childClosestPointIndex = j;
                        minDistance = distance;
                    }
                }
            }

            if (parentClosestPointIndex == 0)
            {
                Vector3 tmp = parent.Start;
                parent.Start = parent.End;
                parent.End = tmp;
            }

            if (childClosestPointIndex == 1)
            {
                Vector3 tmp = child.Start;
                child.Start = child.End;
                child.End = tmp;
            }

            if (minDistance < epsilon)
            {
                Vector3 center = child.Start + (parent.End - child.Start) / 2;
                parent.End = center;
                child.Start = center;
                return true;
            }

            return false;
        }

        private static float SqrDistance(Vector3 lhs, Vector3 rhs)
        {
            float x = rhs.x - lhs.x;
            float y = rhs.y - lhs.y;
            float z = rhs.z - lhs.z;

            return x * x + y * y + z * z;
        }

        #endregion
    }
}

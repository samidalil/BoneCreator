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
        private SkinnedMeshRenderer _skinnedMeshRenderer;

        [SerializeField] private GameObject leftForeleg;
        [SerializeField] private GameObject leftFootStart;
        [SerializeField] private GameObject leftFootEnd;
        [SerializeField] private GameObject rightForeleg;
        [SerializeField] private GameObject rightFootStart;
        [SerializeField] private GameObject rightFootEnd;
        [SerializeField] private GameObject hips;
        [SerializeField] private GameObject leftLeg;
        [SerializeField] private GameObject rightLeg;
        [SerializeField] private GameObject leftForearm;
        [SerializeField] private GameObject leftHandStart;
        [SerializeField] private GameObject leftHandEnd;
        [SerializeField] private GameObject rightForearm;
        [SerializeField] private GameObject rightHandStart;
        [SerializeField] private GameObject rightHandEnd;
        [SerializeField] private GameObject torso;
        [SerializeField] private GameObject leftArm;
        [SerializeField] private GameObject rightArm;
        [SerializeField] private GameObject head;
        [SerializeField] private GameObject headEnd;

        #endregion

        #region Fields

        private List<Transform> _rig = new List<Transform>();

        #endregion

        #region Unity Callbacks

        private void Start()
        {
            Transform[] bones = new Transform[]
            {
                this.hips.transform,
                this.rightLeg.transform,
                this.leftLeg.transform,
                this.headEnd.transform,
                this.head.transform,
                this.leftArm.transform,
                null,
                null,
                null,
                this.leftForeleg.transform,
                this.leftFootStart.transform,
                this.leftFootEnd.transform,
                this.torso.transform,
                this.leftForearm.transform,
                this.leftHandStart.transform,
                null,
                null,
                null,
                this.leftHandEnd.transform,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                this.rightArm.transform,
                this.rightForeleg.transform,
                this.rightFootEnd.transform,
                this.rightFootStart.transform,
                this.rightForearm.transform,
                this.rightHandStart.transform,
                this.rightHandEnd.transform,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null
            };

            this._skinnedMeshRenderer.bones = bones;
        }

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
            Vector3[] rightFootVertices
        )
        {
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

            this.leftForeleg = CreateJoint(null, leftLegPrimaryComponent, leftForelegPrimaryComponent, "Left Foreleg");
            this.leftFootStart = CreateJoint(leftForeleg, leftForelegPrimaryComponent, leftFootPrimaryComponent, "Left Foot Start");
            this.leftFootEnd = CreateJointObject(leftFootPrimaryComponent.End, "Left Foot End", leftFootStart);

            this.rightForeleg = CreateJoint(null, rightLegPrimaryComponent, rightForelegPrimaryComponent, "Right Foreleg");
            this.rightFootStart = CreateJoint(rightForeleg, rightForelegPrimaryComponent, rightFootPrimaryComponent, "Right Foot Start");
            this.rightFootEnd = CreateJointObject(rightFootPrimaryComponent.End, "Right Foot End", rightFootStart);

            Segment hipSegment = new Segment(leftLegPrimaryComponent.Start, rightLegPrimaryComponent.Start);
            this.hips = CreateJointObject(hipSegment.Center, "Hips");
            
            this.leftLeg = CreateJointObject(hipSegment.Start, "Left Leg", hips);
            this.rightLeg = CreateJointObject(hipSegment.End, "Right Leg", hips);

            leftForeleg.transform.SetParent(leftLeg.transform);
            rightForeleg.transform.SetParent(rightLeg.transform);

            // Torso

            this.leftForearm = CreateJoint(null, leftUpperArmPrimaryComponent, leftForearmPrimaryComponent, "Left Forearm");
            this.leftHandStart = CreateJoint(leftForearm, leftForearmPrimaryComponent, leftHandPrimaryComponent, "Left Hand Start");
            this.leftHandEnd = CreateJointObject(leftHandPrimaryComponent.End, "Left Hand End", leftHandStart);

            this.rightForearm = CreateJoint(null, rightUpperArmPrimaryComponent, rightForearmPrimaryComponent, "Right Forearm");
            this.rightHandStart = CreateJoint(rightForearm, rightForearmPrimaryComponent, rightHandPrimaryComponent, "Right Hand Start");
            this.rightHandEnd = CreateJointObject(rightHandPrimaryComponent.End, "Right Hand End", rightHandStart);

            Segment shoulderSegment = new Segment(leftUpperArmPrimaryComponent.Start, rightUpperArmPrimaryComponent.Start);
            this.torso = CreateJointObject(shoulderSegment.Center, "Torso", hips);

            this.leftArm = CreateJointObject(shoulderSegment.Start, "Left Arm", torso);
            this.rightArm = CreateJointObject(shoulderSegment.End, "Right Arm", torso);

            leftForearm.transform.SetParent(leftArm.transform);
            rightForearm.transform.SetParent(rightArm.transform);

            // Head

            this.head = CreateJoint(torso, bodyPrimaryComponent, headPrimaryComponent, "Head Start");
            this.headEnd = CreateJointObject(headPrimaryComponent.End, "Head End", head);

            hips.transform.SetParent(this.transform);
        }

        #endregion

        #region Private Static Methods

        private GameObject CreateJoint(GameObject parentJoint, Segment parent, Segment child, string name)
        {
            return Link(parent, child)
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
        private static bool Link(Segment parent, Segment child)
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

            if (minDistance < 0.1f)
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

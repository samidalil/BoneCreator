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
        [Tooltip("Generated bones")]
        private List<Segment> _components = new List<Segment>();

        [SerializeField]
        [Tooltip("Is Debugging ?")]
        private bool _debug;
        
        [SerializeField]
        [Tooltip("Epsilon used to generate the skeleton")]
        private float _epsilonSquared;

        [SerializeField]
        [Tooltip("Bones list")]
        private List<Transform> _rig = new List<Transform>();

        [SerializeField]
        [Tooltip("Usually hips bone")]
        private Transform _rootBone = null;

        [SerializeField] private SkinnedMeshRenderer _skinnedMeshRenderer;

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

        #region Unity Callbacks

        /// <summary>
        /// Fired on draw gizmos tick
        /// </summary>
        private void OnDrawGizmos()
        {
            if (!this._debug) return;

            Gizmos.color = Color.yellow;

            foreach (Transform bone in this._rig)
                Gizmos.DrawSphere(bone.position, 0.01f);

            Gizmos.color = Color.red;

            foreach (Segment segment in this._components)
                Gizmos.DrawLine(segment.Start, segment.End);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Assigns bones to a Mixamo Skinned Mesh Renderer
        /// </summary>
        public void AssignBones()
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
        /// Initializes the skeleton and its bones
        /// </summary>
        public void Initialize(
            Mesh headMesh,
            Mesh bodyMesh,
            Mesh leftUpperArmMesh,
            Mesh leftForearmMesh,
            Mesh leftHandMesh,
            Mesh rightUpperArmMesh,
            Mesh rightForearmMesh,
            Mesh rightHandMesh,
            Mesh leftLegMesh,
            Mesh leftForelegMesh,
            Mesh leftFootMesh,
            Mesh rightLegMesh,
            Mesh rightForelegMesh,
            Mesh rightFootMesh,
            float epsilon
        )
        {
            this._epsilonSquared = epsilon;

            Debug.Log("TEST");

            Segment headPrimaryComponent = Geometry.GenerateApproximatedPrimaryComponent(headMesh.vertices);
            Segment bodyPrimaryComponent = Geometry.GenerateApproximatedPrimaryComponent(bodyMesh.vertices);
            Segment leftUpperArmPrimaryComponent = Geometry.GenerateApproximatedPrimaryComponent(leftUpperArmMesh.vertices);
            Segment leftForearmPrimaryComponent = Geometry.GenerateApproximatedPrimaryComponent(leftForearmMesh.vertices);
            Segment leftHandPrimaryComponent = Geometry.GenerateApproximatedPrimaryComponent(leftHandMesh.vertices);
            Segment rightUpperArmPrimaryComponent = Geometry.GenerateApproximatedPrimaryComponent(rightUpperArmMesh.vertices);
            Segment rightForearmPrimaryComponent = Geometry.GenerateApproximatedPrimaryComponent(rightForearmMesh.vertices);
            Segment rightHandPrimaryComponent = Geometry.GenerateApproximatedPrimaryComponent(rightHandMesh.vertices);
            Segment leftLegPrimaryComponent = Geometry.GenerateApproximatedPrimaryComponent(leftLegMesh.vertices);
            Segment leftForelegPrimaryComponent = Geometry.GenerateApproximatedPrimaryComponent(leftForelegMesh.vertices);
            Segment leftFootPrimaryComponent = Geometry.GenerateApproximatedPrimaryComponent(leftFootMesh.vertices);
            Segment rightLegPrimaryComponent = Geometry.GenerateApproximatedPrimaryComponent(rightLegMesh.vertices);
            Segment rightForelegPrimaryComponent = Geometry.GenerateApproximatedPrimaryComponent(rightForelegMesh.vertices);
            Segment rightFootPrimaryComponent = Geometry.GenerateApproximatedPrimaryComponent(rightFootMesh.vertices);

            // Hips

            Link(leftLegPrimaryComponent, leftForelegPrimaryComponent, epsilon);
            Link(rightLegPrimaryComponent, rightForelegPrimaryComponent, epsilon);

            Segment hipSegment = new Segment(leftLegPrimaryComponent.Start, rightLegPrimaryComponent.Start);
            this.hips = CreateJointObject(hipSegment.Center, null, "Hips");

            this.leftLeg = CreateJointObject(hipSegment.Start, leftLegMesh, "Left Leg", hips);
            this.leftForeleg = CreateJoint(leftLeg, leftLegPrimaryComponent, leftForelegPrimaryComponent, leftForelegMesh, "Left Foreleg");
            this.leftFootStart = CreateJoint(leftForeleg, leftForelegPrimaryComponent, leftFootPrimaryComponent, leftFootMesh, "Left Foot Start");
            this.leftFootEnd = CreateJointObject(leftFootPrimaryComponent.End, null, "Left Foot End", leftFootStart);

            this.rightLeg = CreateJointObject(hipSegment.End, rightLegMesh, "Right Leg", hips);
            this.rightForeleg = CreateJoint(rightLeg, rightLegPrimaryComponent, rightForelegPrimaryComponent, rightForelegMesh, "Right Foreleg");
            this.rightFootStart = CreateJoint(rightForeleg, rightForelegPrimaryComponent, rightFootPrimaryComponent, rightFootMesh, "Right Foot Start");
            this.rightFootEnd = CreateJointObject(rightFootPrimaryComponent.End, null, "Right Foot End", rightFootStart);

            // Torso

            Link(leftUpperArmPrimaryComponent, leftForearmPrimaryComponent, epsilon);
            Link(rightUpperArmPrimaryComponent, rightForearmPrimaryComponent, epsilon);

            Segment shoulderSegment = new Segment(leftUpperArmPrimaryComponent.Start, rightUpperArmPrimaryComponent.Start);
            this.torso = CreateJointObject(shoulderSegment.Center, bodyMesh, "Torso", hips);

            this.leftArm = CreateJointObject(shoulderSegment.Start, leftUpperArmMesh, "Left Arm", torso);
            this.leftForearm = CreateJoint(leftArm, leftUpperArmPrimaryComponent, leftForearmPrimaryComponent, leftForearmMesh, "Left Forearm");
            this.leftHandStart = CreateJoint(leftForearm, leftForearmPrimaryComponent, leftHandPrimaryComponent, leftHandMesh, "Left Hand Start");
            this.leftHandEnd = CreateJointObject(leftHandPrimaryComponent.End, null, "Left Hand End", leftHandStart);

            this.rightArm = CreateJointObject(shoulderSegment.End, rightUpperArmMesh, "Right Arm", torso);
            this.rightForearm = CreateJoint(rightArm, rightUpperArmPrimaryComponent, rightForearmPrimaryComponent, rightForearmMesh, "Right Forearm");
            this.rightHandStart = CreateJoint(rightForearm, rightForearmPrimaryComponent, rightHandPrimaryComponent, rightHandMesh, "Right Hand Start");
            this.rightHandEnd = CreateJointObject(rightHandPrimaryComponent.End, null, "Right Hand End", rightHandStart);

            // Head

            this.head = CreateJoint(torso, bodyPrimaryComponent, headPrimaryComponent, headMesh, "Head Start");
            this.headEnd = CreateJointObject(headPrimaryComponent.End, null, "Head End", head);

            hips.transform.SetParent(this.transform);

            this._rootBone = hips.transform;
        }

        #endregion

        #region Private Static Methods

        private GameObject CreateJoint(GameObject parentJoint, Segment parent, Segment child, Mesh mesh, string name)
        {
            return Link(parent, child, this._epsilonSquared)
                ? CreateJointObject(parent.End, mesh, name, parentJoint)
                : CreateJointObject(child.Start, mesh, name, CreateJointObject(child.End, mesh, $"intermediate-{name}", parentJoint));
        }

        private GameObject CreateJointObject(Vector3 position, Mesh mesh, string name, GameObject parent = null)
        {
            GameObject joint = GameObject.CreatePrimitive(PrimitiveType.Sphere);

            joint.transform.localScale = Vector3.one * 0.2f;
            joint.name = name;
            joint.transform.position = position;
            
            this._rig.Add(joint.transform);

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

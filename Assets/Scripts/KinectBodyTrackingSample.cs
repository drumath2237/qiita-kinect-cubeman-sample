using System.Collections.Generic;
using System.Linq;
using Kinect = Windows.Kinect;
using UnityEngine;

[RequireComponent(typeof(BodySourceManager))]
public class KinectBodyTrackingSample : MonoBehaviour
{
    private BodySourceManager _BodySource;

    /// <summary>
    /// jointデータを格納しているBodyデータ
    /// </summary>
    private Kinect.Body _Body;

    /// <summary>
    /// cubemanの関節
    /// </summary>
    private GameObject 
        _head,
        _leftHand,
        _rightHand,
        _spineShoulder,
        _spineBase,
        _leftFoot,
        _rightFoot;

    private Dictionary<GameObject, Kinect.JointType> ObjectJointTable;

    void Start()
    {
        _BodySource = GetComponent<BodySourceManager>();
        
        #region cubeman
        _head = GameObject.CreatePrimitive(PrimitiveType.Cube);
        _spineShoulder = GameObject.CreatePrimitive(PrimitiveType.Cube);
        _leftHand = GameObject.CreatePrimitive(PrimitiveType.Cube);
        _rightHand = GameObject.CreatePrimitive(PrimitiveType.Cube);
        _spineBase = GameObject.CreatePrimitive(PrimitiveType.Cube);
        _leftFoot = GameObject.CreatePrimitive(PrimitiveType.Cube);
        _rightFoot = GameObject.CreatePrimitive(PrimitiveType.Cube);
        #endregion cubeman
        
        ObjectJointTable = new Dictionary<GameObject, Kinect.JointType>() {
            {_head, Kinect.JointType.Head},
            {_spineShoulder, Kinect.JointType.SpineShoulder},
            {_leftHand, Kinect.JointType.HandLeft},
            {_rightHand, Kinect.JointType.HandRight},
            {_spineBase, Kinect.JointType.SpineBase},
            {_leftFoot, Kinect.JointType.FootLeft},
            {_rightFoot, Kinect.JointType.FootRight}
        };
    }

    void Update()
    {
        _Body = _BodySource.GetData().FirstOrDefault(b => b.IsTracked);
        if (!_Body.IsTracked)
            return;

        SetPose(_head);
        SetPose(_spineShoulder);
        SetPose(_leftHand);
        SetPose(_rightHand);
        SetPose(_spineBase);
        SetPose(_leftFoot);
        SetPose(_rightFoot);
        
    }

    /// <summary>
    /// cubemanのそれぞれの関節にjointの姿勢データを適用する
    /// </summary>
    /// <param name="obj">cubemanの関節</param>
    void SetPose(GameObject obj)
    {
        obj.transform.position = GetJointPose(ObjectJointTable[obj]).position;
        obj.transform.rotation = GetJointPose(ObjectJointTable[obj]).rotation;
        obj.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        obj.name = ObjectJointTable[obj].ToString();
    }

    /// <summary>
    /// Jointの位置と回転をPose構造体として返す
    /// </summary>
    /// <param name="type">jointの種類</param>
    /// <returns>jointの位置と回転データ</returns>
    Pose GetJointPose(Kinect.JointType type)
    {
        Pose pose = Pose.identity;
        pose.position = new Vector3(
            _Body.Joints[type].Position.X,
            _Body.Joints[type].Position.Y,
            _Body.Joints[type].Position.Z
        );
        pose.rotation = new Quaternion(
            _Body.JointOrientations[type].Orientation.X,
            _Body.JointOrientations[type].Orientation.Y,
            _Body.JointOrientations[type].Orientation.Z,
            _Body.JointOrientations[type].Orientation.W
        );

        return pose;
    }
}

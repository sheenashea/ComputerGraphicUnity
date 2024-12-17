using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Compass : MonoBehaviour
{
    public Transform mPlayerTransform;
    
    [Tooltip("The direction towards which the compass points. Default for North is (0, 0, 1)")]
    public Vector3 kReferenceVector = new Vector3(0, 0, 1);
    
  
    private Vector3 _mTempVector;
    private float _mTempAngle;

    //Update每帧调用一次
    private void Update()
    {
        // 获取玩家的transform，将y设置为0并将其归一化
        _mTempVector = mPlayerTransform.forward;
        _mTempVector.y = 0f;
        _mTempVector = _mTempVector.normalized;

        // 获取到参考点的距离，确保y等于0并将其归一化（normalize）
        _mTempVector = _mTempVector - kReferenceVector;
        _mTempVector.y = 0;
        _mTempVector = _mTempVector.normalized;

        // 如果两个向量之间的距离为0，则会导致后续角度计算出现问题
        if (_mTempVector == Vector3.zero)
        {
            _mTempVector = new Vector3(1, 0, 0);
        }

        // 计算旋转角度（弧度），并进行调整
        _mTempAngle = Mathf.Atan2(_mTempVector.x, _mTempVector.z);
        _mTempAngle = (_mTempAngle * Mathf.Rad2Deg + 90f) * 2f;

        // 设置旋转
        transform.rotation = Quaternion.AngleAxis(_mTempAngle, kReferenceVector);

    }
}

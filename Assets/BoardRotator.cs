using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardRotator : MonoBehaviour
{
    public float duration = 1.0f;

    private float mRotation;
    [SerializeField]
    private float mTargetRotation;
    private float mCumulator = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        mRotation = transform.eulerAngles.y;
        mTargetRotation = mRotation;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.D) && !Rotating())
        {
            mTargetRotation += 90f;
        }
        if (Input.GetKeyDown(KeyCode.Q) && !Rotating())
        {
            mTargetRotation -= 90.0f;
        }

        if (Rotating())
        {
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, Mathf.LerpAngle(mRotation, mTargetRotation, Mathf.SmoothStep(0.0f, 1.0f, mCumulator)), transform.eulerAngles.z);

            mCumulator += Time.deltaTime / duration;
        }
        else
        {
            mRotation = mTargetRotation;
            mCumulator = 0.0f;
        }
    }

    private bool Rotating()
    {
        return mRotation != mTargetRotation && mCumulator < 1.0f;
    }
}

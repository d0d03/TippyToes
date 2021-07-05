using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public event System.Action OnReachedEndOfLvl;

    public float moveSpeed = 7;
    public float smoothMoveTime = .1f;
    public float turnSpeed = 8;

    float angle;
    float smoothInputMagnitude;
    float smoothMoveVelocity;
    Vector3 velocity;

    Rigidbody rigidbody;
    bool disabled;
    bool isWalking;

    Animator m_animator;
    AudioSource footstepsAudio;

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        m_animator = GetComponent<Animator>();
        footstepsAudio = GetComponent<AudioSource>();
        Guard.OnGuardHasSpotedPlayer += Disable;
        Door.EnterMinigame += Disable;
        Door.ExitMinigame += Enable;
        EMP.EnterMinigame += Disable;
        EMP.ExitMinigame += Enable;
        
    }

    //void Update()
    //{
    //    Vector3 inputDirection = Vector3.zero;
    //    if (!disabled) {
    //        inputDirection = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
    //    }
    //    float inputMagnitude = inputDirection.magnitude;

    //    smoothInputMagnitude = Mathf.SmoothDamp(smoothInputMagnitude, inputMagnitude, ref smoothMoveVelocity, smoothMoveTime);

    //    float targetAngle = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg;
    //    angle = Mathf.LerpAngle(angle, targetAngle, Time.deltaTime * turnSpeed * inputMagnitude);
    //    velocity = transform.forward * moveSpeed * smoothInputMagnitude;
    //}

    private void OnTriggerEnter(Collider hitCollider)
    {
        if (hitCollider.tag == "Finish") {
            Disable();
            if (OnReachedEndOfLvl != null) {
                OnReachedEndOfLvl();  
            }
        }
    }

    void Disable() {
        disabled = true;
    }

    void Enable() {
        disabled = false;
    }

    void Update()
    {
        Vector3 inputDirection = Vector3.zero;
        

        if (!disabled)
        {
            inputDirection = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
            isWalking = inputDirection != Vector3.zero || false;
        }
        else {
            isWalking = false;
        }
        float inputMagnitude = inputDirection.magnitude;

        smoothInputMagnitude = Mathf.SmoothDamp(smoothInputMagnitude, inputMagnitude, ref smoothMoveVelocity, smoothMoveTime);

        float targetAngle = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg;
        angle = Mathf.LerpAngle(angle, targetAngle, Time.deltaTime * turnSpeed * inputMagnitude);
        velocity = transform.forward * moveSpeed * smoothInputMagnitude;
        m_animator.SetBool("IsWalking", isWalking);
    }

    void OnDestroy()
    {
        Guard.OnGuardHasSpotedPlayer -= Disable;
        Door.EnterMinigame -= Disable;
        Door.ExitMinigame -= Enable;
        EMP.EnterMinigame -= Disable;
        EMP.ExitMinigame -= Enable;
    }

    private void OnAnimatorMove()
    {
        rigidbody.MoveRotation(Quaternion.Euler(Vector3.up * angle));
        rigidbody.MovePosition(rigidbody.position + velocity * Time.deltaTime);
        if (isWalking)
        {
            if (!footstepsAudio.isPlaying)
            {
                footstepsAudio.Play();
            }
        }
        else {
            footstepsAudio.Stop();
        }
        
    }
}

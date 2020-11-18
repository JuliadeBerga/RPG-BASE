using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

//Script assignat al Player

public class PlayerControllerBasic : MonoBehaviour
{
    #region Private Members

    private Animator _animator;

    //Variable que necessita afegir la component CharacterController al Player
    private CharacterController _characterController;

    private float Gravity = 20.0f;

    //Vector3 utilitzat per el moviment. Si és en l'eix X serà moure's; si és en l'eix y serà per saltar
    private Vector3 _moveDirection = Vector3.zero;

    #endregion

    #region Public Members

    public float Speed = 5.0f;

    public float RotationSpeed = 240.0f;

    public float JumpSpeed = 7.0f;

    #endregion

    // Use this for initialization
    void Start()
    {
        //Inicialitzem la variable animator
        _animator = GetComponent<Animator>();

        //Inicialitzem la variable CaracterController
        _characterController = GetComponent<CharacterController>();
    }
 

    private bool mIsControlEnabled = true;

    public void EnableControl()
    {
        mIsControlEnabled = true;
    }

    public void DisableControl()
    {
        mIsControlEnabled = false;
    }

    // Update és una funció que s'executa a cada frame
    //Activació de l'animació d'atacar
    //Control del moviment ---MOLT IMPORTANT---
    void Update()
    {
        if (mIsControlEnabled)
        {
            //Quan es premin les fletxes de 'Horitzontal' i 'Vertical' o WASD
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");

            //Calcula del vector de la camera
            Vector3 camForward_Dir = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;

            //Calcula el vector d'anar endavant del Jugador
            Vector3 move = v * camForward_Dir + h * Camera.main.transform.right;

            //Si hi ha valors a la variable  Vector3, mou el player
            if (move.magnitude > 1f) move.Normalize();

            //Calcula la rotació del player
            move = transform.InverseTransformDirection(move);

            //Obtenir els angles Eules
            float turnAmount = Mathf.Atan2(move.x, move.z);

            transform.Rotate(0, turnAmount * RotationSpeed * Time.deltaTime, 0);

            //Si el jugador està a terra
            if (_characterController.isGrounded)
            {
                //Mou el jugador
                _moveDirection = transform.forward * move.magnitude;

                //Multiplica aquest moviment per la velocitat (speed)
                _moveDirection *= Speed;

                //Si premem la techa "Jump"
                //"Jump" està definit a Edit->Project Settings->Input->Axes->Jump
                if (Input.GetButton("Jump"))
                {
                    //Activa l'animació de Sañtar
                    _animator.SetBool("is_in_air", true);

                    //Fer saltar al player donant-li un valor a l'eix Y (vertical) al playerr
                    _moveDirection.y = JumpSpeed;

                }
                else
                {
                    //Desactivar animació de Saltar
                    _animator.SetBool("is_in_air", false);

                    //Activar animació de Correr
                    _animator.SetBool("run", move.magnitude > 0);
                }
            }

            //Resta la gravetat perquè no es quedi flotant a l'aire
            _moveDirection.y -= Gravity * Time.deltaTime;

            //Moure el jugador
            _characterController.Move(_moveDirection * Time.deltaTime);
        }
    }

    
}

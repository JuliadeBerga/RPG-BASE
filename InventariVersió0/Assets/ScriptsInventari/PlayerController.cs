using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

//Script assignat al Player

public class PlayerController : MonoBehaviour
{
    #region Private Members

    private Animator _animator;

    //Variable que necessita afegir la component CharacterController al Player
    private CharacterController _characterController;

    private float Gravity = 20.0f;

    //Vector3 utilitzat per el moviment. Si és en l'eix X serà moure's; si és en l'eix y serà per saltar
    private Vector3 _moveDirection = Vector3.zero;

    //Inicialitzem el mCurrentItem com a buit. Aquesta variable la utilitzarem per fer el Drop
    private InventoryItemBase mCurrentItem = null;

    private HealthBar mHealthBar;

    private HealthBar mFoodBar;

    private int startHealth;

    private int startFood;

    #endregion

    #region Public Members

    public float Speed = 5.0f;

    public float RotationSpeed = 240.0f;

    //Variable per connectar amb l'script Inventory
    public Inventory Inventory;

    //Variable per saber posició de la mà, una vegada agafem l'objecte
    public GameObject Hand;

    //Variable que s'utilitza per connectar amb l'inventari
    public HUD Hud;

    public float JumpSpeed = 7.0f;

    #endregion

    // Use this for initialization
    void Start()
    {
        //Inicialitzem la variable animator
        _animator = GetComponent<Animator>();

        //Inicialitzem la variable CaracterController
        _characterController = GetComponent<CharacterController>();

        //Cridem a l'event Item Used
        Inventory.ItemUsed += Inventory_ItemUsed;

        //Cridem a l'event Item Removed
        Inventory.ItemRemoved += Inventory_ItemRemoved;

               
        mHealthBar = Hud.transform.Find("Bars_Panel/HealthBar").GetComponent<HealthBar>();
        mHealthBar.Min = 0;
        mHealthBar.Max = Health;
        startHealth = Health;
        mHealthBar.SetValue(Health);

        mFoodBar = Hud.transform.Find("Bars_Panel/FoodBar").GetComponent<HealthBar>();
        mFoodBar.Min = 0;
        mFoodBar.Max = Food;
        startFood = Food;
        mFoodBar.SetValue(Food);

        InvokeRepeating("IncreaseHunger", 0, HungerRate);
        
    }

        
    #region Inventory

    //Funció generada automàticament quan al Start hem escrit: Inventory.ItemRemoved += Inventory_ItemRemoved;
    private void Inventory_ItemRemoved(object sender, InventoryEventArgs e)
    {
        InventoryItemBase item = e.Item;

        GameObject goItem = (item as MonoBehaviour).gameObject;
        goItem.SetActive(true);
        goItem.transform.parent = null;

    }

    //Funció per establir quin és el item actiu. Quan cridem a la funció li enviem item i veritat o false en active
    private void SetItemActive(InventoryItemBase item, bool active)
    {
        //Definim item
        GameObject currentItem = (item as MonoBehaviour).gameObject;
        //Activem o Desactivem item (depèn del que haguem enviat)
        currentItem.SetActive(active);
        //Si esta actiu la seva posició serà la de la mà, si no, null
        currentItem.transform.parent = active ? Hand.transform : null;
    }

    //Funció generada automàticament quan al Start hem escrit: Inventory.ItemUsed += Inventory_ItemUsed;
    private void Inventory_ItemUsed(object sender, InventoryEventArgs e)
    {
        //Mira si el item no es consumible 'Consumable'
        if (e.Item.ItemType != EItemType.Consumable)
        {
            //Mira si el jugador ja porta un objecte
            if (mCurrentItem != null)
            {
                //si ja el porta, l'elimina (SetItemActive és una funció que està una mica més amunt)
                SetItemActive(mCurrentItem, false);
            }

            //Va a buscar el nou item
            InventoryItemBase item = e.Item;

            //Activa el nou item a la mà del player (SetItemActive és una funció que està una mica més amunt)
            SetItemActive(item, true);

            //I posa el nou item com a current (l'actual)
            mCurrentItem = e.Item;
        }

    }

    private int Attack_1_Hash = Animator.StringToHash("Base Layer.Attack_1");

    public bool IsAttacking
    {
        get
        {
            AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
            if (stateInfo.fullPathHash == Attack_1_Hash)
            {
                return true;
            }
            return false;
        }
    }

    //Aquesta funció deixa anar l'objecte que tenim a les mans (Fer un Drop)
    public void DropCurrentItem()
    {
        //Primer s'activa l'animació del Drop
        _animator.SetTrigger("tr_drop");

        //Posa la variable GameObject goItem el gameobject que tenia abans l'objecte
        GameObject goItem = (mCurrentItem as MonoBehaviour).gameObject;

        //Crida a la funció RemoItem per eliminar el item del invenrair
        Inventory.RemoveItem(mCurrentItem);

        //Torna a donar-li Rigidbody al item (a la variable rbItem)
        Rigidbody rbItem = goItem.AddComponent<Rigidbody>();


        //Si té rigidbody
        if (rbItem != null)
        {

            rbItem.AddForce(transform.forward * 2.0f, ForceMode.Impulse);

            //Crida 'Invoca' a la funció DoDropItem, explicada a posteriori, després de 0,25 segons
            Invoke("DoDropItem", 0.25f);
        }

    }

    //Eliminar el mCurrentItem (tant el rigidbody, com el objecte)
    public void DoDropItem()
    {

        //Destrueix el Rigidbody
        Destroy((mCurrentItem as MonoBehaviour).GetComponent<Rigidbody>());

        //Posa la variable mCurrentItem a zero
        mCurrentItem = null;
    }

    #endregion

    #region Health & Hunger

    [Tooltip("Amount of health")]
    public int Health = 100;

    [Tooltip("Amount of food")]
    public int Food = 100;

    [Tooltip("Rate in seconds in which the hunger increases")]
    public float HungerRate = 0.5f;

    public void IncreaseHunger()
    {
        Food--;
        if (Food < 0)
            Food = 0;

        mFoodBar.SetValue(Food);

        if (IsDead)
        {
            CancelInvoke();
            _animator.SetTrigger("death");
        }
    }

    public bool IsDead
    {
        get
        {
            return Health == 0 || Food == 0;
        }
    }

    public bool IsArmed
    {
        get
        {
            if (mCurrentItem == null)
                return false;

            return mCurrentItem.ItemType == EItemType.Weapon;
        }
    }


    public void Eat(int amount)
    {
        Food += amount;
        if (Food > startFood)
        {
            Food = startFood;
        }

        mFoodBar.SetValue(Food);

    }

    public void Rehab(int amount)
    {
        Health += amount;
        if (Health > startHealth)
        {
            Health = startHealth;
        }

        mHealthBar.SetValue(Health);
    }

    public void TakeDamage(int amount)
    {
        Health -= amount;
        if (Health < 0)
            Health = 0;

        mHealthBar.SetValue(Health);

        if (IsDead)
        {
            _animator.SetTrigger("death");
        }

    }

    #endregion


    //El següent FixedUpdate és per deixar anar l'objecte agagat (fer un Drop)
    void FixedUpdate()
    {
        //Si el player no està mort
        if (!IsDead)
        {
            //Si tenim un objecte a la mà i cliquem la tecla R
            if (mCurrentItem != null && Input.GetKeyDown(KeyCode.R))
            {
                //Cridem aquesta funció
                DropCurrentItem();
            }
        }
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
        if (!IsDead && mIsControlEnabled)
        {
            //Si estem interactuant amb un objecte i premem la tecla F
            if (mInteractItem != null && Input.GetKeyDown(KeyCode.F))
            {
                // Activar la animació d'agafar item
                mInteractItem.OnInteractAnimation(_animator);
            }

            // Executa l'ordre d'atacar sempre i quan hi hagi un item i es premi el botó esquerre del mouse
            if (mCurrentItem != null && Input.GetMouseButtonDown(0))
            {
                //No executar l'atac si el punter del ratolí està sobre el inventari
                if (!EventSystem.current.IsPointerOverGameObject())
                {
                    //Activar l'animació d'atacar
                    _animator.SetTrigger("attack_1");
                }
            }

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

    public void InteractWithItem()
    {
        if (mInteractItem != null)
        {
            mInteractItem.OnInteract();

            if (mInteractItem is InventoryItemBase)
            {
                InventoryItemBase inventoryItem = mInteractItem as InventoryItemBase;
                Inventory.AddItem(inventoryItem);
                inventoryItem.OnPickup();

                if (inventoryItem.UseItemAfterPickup)
                {
                    Inventory.UseItem(inventoryItem);
                }
            }
        }

        Hud.CloseMessagePanel();

        mInteractItem = null;
    }

    //Defineixo la variable mInteractItem. Aquesta variable ens permetrà saber si estem interactuant amb algun objecte (si ho fem, el podrem agafar)
    private InteractableItemBase mInteractItem = null;

    //Funció per posar a la variable Item l'objecte interactuat i connectar amb l'inventari (HUD)
    private void OnTriggerEnter(Collider other)
    {
        //Inicialitzem la variable item
        InteractableItemBase item = other.GetComponent<InteractableItemBase>();

        //si hi ha un item
        if (item != null)
        {
            //si amb aquest item es pot interactuar
            if (item.CanInteract(other))
            {
                // Activem la variable mInteractItem amb l'item
                mInteractItem = item;

                //Cridem a la funció que aparegui el missatge per pantalla passant-li el paràmetre item (mInteractItem)
                Hud.OpenMessagePanel(mInteractItem);
            }
        }
    }

    //Funció per acabar la funció anterior OnTriggerEnter, una vegada ja no hi hagi trigger
    private void OnTriggerExit(Collider other)
    {
        //Inicialitzem la variable item
        InteractableItemBase item = other.GetComponent<InteractableItemBase>();

        //si hi ha un item
        if (item != null)
        {
            //Cridem a la funció que desaparegui el missatge per pantalla
            Hud.CloseMessagePanel();

            //Posem a buit la variable mInteractItem, ja que no hi ha objecte amb qui interactua
            mInteractItem = null;
        }
    }
}

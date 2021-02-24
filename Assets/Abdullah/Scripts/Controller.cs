// GENERATED AUTOMATICALLY FROM 'Assets/Abdullah/Scripts/Controller.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @Controller : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @Controller()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""Controller"",
    ""maps"": [
        {
            ""name"": ""Player"",
            ""id"": ""18ed9aa0-66cb-4a16-8e1d-837f3d1680d1"",
            ""actions"": [
                {
                    ""name"": ""Movement"",
                    ""type"": ""PassThrough"",
                    ""id"": ""b8228302-1c86-4e08-af05-88241b1a8e9c"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Jump"",
                    ""type"": ""Button"",
                    ""id"": ""5c517d44-8d6f-45b3-957f-cd56c7320f5a"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Look"",
                    ""type"": ""PassThrough"",
                    ""id"": ""b7a6a53f-82ed-4471-a414-771b8e7b1c5b"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Dash"",
                    ""type"": ""Button"",
                    ""id"": ""919c190c-3357-40cf-9ef0-b6d59b807e0d"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""TestButton2"",
                    ""type"": ""Button"",
                    ""id"": ""57bc4b99-5690-4585-bedd-a50e14320292"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""TestButton1"",
                    ""type"": ""Button"",
                    ""id"": ""f8d11d9b-a054-4998-b3d1-7c211656a5db"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""DespawnAllGuns"",
                    ""type"": ""Button"",
                    ""id"": ""e797ec15-e36e-45b4-ba7a-78a36d7be230"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""GenerateGun"",
                    ""type"": ""Button"",
                    ""id"": ""25cd3158-9f23-4c28-93e0-e5f1513384a5"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Move"",
                    ""type"": ""Value"",
                    ""id"": ""63245073-fff7-438c-ab10-8b9c87ee2cb4"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Reload"",
                    ""type"": ""Button"",
                    ""id"": ""75f03887-38b3-493a-b7d0-2c5d314f0299"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Aim"",
                    ""type"": ""Button"",
                    ""id"": ""4e404d22-2e26-4e55-be7d-10f27e2f20a8"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Shoot"",
                    ""type"": ""Button"",
                    ""id"": ""535c80b3-6be2-4576-b092-5219091c8d8b"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Gun1"",
                    ""type"": ""Button"",
                    ""id"": ""eb5ad711-53d4-4bd9-b8e9-3690807e52a7"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Gun2"",
                    ""type"": ""Button"",
                    ""id"": ""79f66fee-a87c-41bf-b744-573456157e79"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Gun3"",
                    ""type"": ""Button"",
                    ""id"": ""87814599-2987-4f65-b398-fc9e3cf42212"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""WeaponWheel"",
                    ""type"": ""Button"",
                    ""id"": ""4fa00674-e281-4224-b4a0-bd18f29d7f49"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""WASD"",
                    ""id"": ""cfacdf24-8ef4-4ee8-a43a-f624589d9804"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""8e78fca9-bbda-49a0-9d2b-c749a0a2abbc"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""7dc5a82e-7cd0-4cea-8b31-b7150b7aae99"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""93ffb390-9724-4081-b632-d8ddd8fd2865"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""4e710641-e640-4b6f-9f39-1d31f3cf8b34"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""73765232-c756-4cd1-ba37-917cda663eae"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""df14e466-f2a8-433c-9c63-3d4c50dbf575"",
                    ""path"": ""<Mouse>/delta"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Look"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""791f43b8-185c-4d52-998d-319947fd1b95"",
                    ""path"": ""<Keyboard>/shift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Dash"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""890f018a-befc-4b39-b9d3-a7709b1bd566"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Shoot"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a944a15f-17ee-4803-971f-7f670ad87214"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Aim"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""7f54000f-da5d-498a-aaed-a1bed0258dda"",
                    ""path"": ""<Keyboard>/r"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Reload"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""2D Vector"",
                    ""id"": ""e1852722-69f5-4d8c-a676-dae3957cd107"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""72df0ed5-039d-4d3e-a072-ceaba54b6c66"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""dfcbf6f5-08c7-4a6d-b153-e3dadbfe0aaa"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""6ad539d9-a234-4e7a-b62c-31ef1871e37f"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""878a7657-d29c-424a-ac2e-b0ee7fcbf5ae"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""751f7beb-3cd7-464c-8b3b-5b373f83b33c"",
                    ""path"": ""<Keyboard>/v"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""DespawnAllGuns"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a015f36c-448f-4d3f-ae58-a009b3d92ed0"",
                    ""path"": ""<Keyboard>/t"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""TestButton1"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f51a97e9-ae20-4e4a-9cc8-85ef290fb5a3"",
                    ""path"": ""<Keyboard>/y"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""TestButton2"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""8cf02ec4-7f4f-422b-8242-26cc9108d59e"",
                    ""path"": ""<Keyboard>/c"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""GenerateGun"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""tempName"",
                    ""id"": ""4102c0ab-230d-414f-9833-49020dd52470"",
                    ""path"": """",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": """",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a5904da9-fd76-44ca-9bde-c0011ff550f8"",
                    ""path"": ""<Keyboard>/1"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Gun1"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""92fc7e32-31d9-43cc-9eb7-39cbd31792c1"",
                    ""path"": ""<Keyboard>/2"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Gun2"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""24625dc5-3f49-47d9-9de0-813dda84359e"",
                    ""path"": ""<Keyboard>/3"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Gun3"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""2ae088d5-e3b5-4355-b16d-de2989c281fc"",
                    ""path"": ""<Keyboard>/tab"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""M&K"",
                    ""action"": ""WeaponWheel"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""M&K"",
            ""bindingGroup"": ""M&K"",
            ""devices"": [
                {
                    ""devicePath"": ""<Mouse>"",
                    ""isOptional"": false,
                    ""isOR"": false
                },
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
        // Player
        m_Player = asset.FindActionMap("Player", throwIfNotFound: true);
        m_Player_Movement = m_Player.FindAction("Movement", throwIfNotFound: true);
        m_Player_Jump = m_Player.FindAction("Jump", throwIfNotFound: true);
        m_Player_Look = m_Player.FindAction("Look", throwIfNotFound: true);
        m_Player_Dash = m_Player.FindAction("Dash", throwIfNotFound: true);
        m_Player_TestButton2 = m_Player.FindAction("TestButton2", throwIfNotFound: true);
        m_Player_TestButton1 = m_Player.FindAction("TestButton1", throwIfNotFound: true);
        m_Player_DespawnAllGuns = m_Player.FindAction("DespawnAllGuns", throwIfNotFound: true);
        m_Player_GenerateGun = m_Player.FindAction("GenerateGun", throwIfNotFound: true);
        m_Player_Move = m_Player.FindAction("Move", throwIfNotFound: true);
        m_Player_Reload = m_Player.FindAction("Reload", throwIfNotFound: true);
        m_Player_Aim = m_Player.FindAction("Aim", throwIfNotFound: true);
        m_Player_Shoot = m_Player.FindAction("Shoot", throwIfNotFound: true);
        m_Player_Gun1 = m_Player.FindAction("Gun1", throwIfNotFound: true);
        m_Player_Gun2 = m_Player.FindAction("Gun2", throwIfNotFound: true);
        m_Player_Gun3 = m_Player.FindAction("Gun3", throwIfNotFound: true);
        m_Player_WeaponWheel = m_Player.FindAction("WeaponWheel", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }

    // Player
    private readonly InputActionMap m_Player;
    private IPlayerActions m_PlayerActionsCallbackInterface;
    private readonly InputAction m_Player_Movement;
    private readonly InputAction m_Player_Jump;
    private readonly InputAction m_Player_Look;
    private readonly InputAction m_Player_Dash;
    private readonly InputAction m_Player_TestButton2;
    private readonly InputAction m_Player_TestButton1;
    private readonly InputAction m_Player_DespawnAllGuns;
    private readonly InputAction m_Player_GenerateGun;
    private readonly InputAction m_Player_Move;
    private readonly InputAction m_Player_Reload;
    private readonly InputAction m_Player_Aim;
    private readonly InputAction m_Player_Shoot;
    private readonly InputAction m_Player_Gun1;
    private readonly InputAction m_Player_Gun2;
    private readonly InputAction m_Player_Gun3;
    private readonly InputAction m_Player_WeaponWheel;
    public struct PlayerActions
    {
        private @Controller m_Wrapper;
        public PlayerActions(@Controller wrapper) { m_Wrapper = wrapper; }
        public InputAction @Movement => m_Wrapper.m_Player_Movement;
        public InputAction @Jump => m_Wrapper.m_Player_Jump;
        public InputAction @Look => m_Wrapper.m_Player_Look;
        public InputAction @Dash => m_Wrapper.m_Player_Dash;
        public InputAction @TestButton2 => m_Wrapper.m_Player_TestButton2;
        public InputAction @TestButton1 => m_Wrapper.m_Player_TestButton1;
        public InputAction @DespawnAllGuns => m_Wrapper.m_Player_DespawnAllGuns;
        public InputAction @GenerateGun => m_Wrapper.m_Player_GenerateGun;
        public InputAction @Move => m_Wrapper.m_Player_Move;
        public InputAction @Reload => m_Wrapper.m_Player_Reload;
        public InputAction @Aim => m_Wrapper.m_Player_Aim;
        public InputAction @Shoot => m_Wrapper.m_Player_Shoot;
        public InputAction @Gun1 => m_Wrapper.m_Player_Gun1;
        public InputAction @Gun2 => m_Wrapper.m_Player_Gun2;
        public InputAction @Gun3 => m_Wrapper.m_Player_Gun3;
        public InputAction @WeaponWheel => m_Wrapper.m_Player_WeaponWheel;
        public InputActionMap Get() { return m_Wrapper.m_Player; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayerActions set) { return set.Get(); }
        public void SetCallbacks(IPlayerActions instance)
        {
            if (m_Wrapper.m_PlayerActionsCallbackInterface != null)
            {
                @Movement.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMovement;
                @Movement.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMovement;
                @Movement.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMovement;
                @Jump.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnJump;
                @Jump.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnJump;
                @Jump.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnJump;
                @Look.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnLook;
                @Look.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnLook;
                @Look.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnLook;
                @Dash.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnDash;
                @Dash.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnDash;
                @Dash.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnDash;
                @TestButton2.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnTestButton2;
                @TestButton2.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnTestButton2;
                @TestButton2.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnTestButton2;
                @TestButton1.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnTestButton1;
                @TestButton1.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnTestButton1;
                @TestButton1.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnTestButton1;
                @DespawnAllGuns.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnDespawnAllGuns;
                @DespawnAllGuns.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnDespawnAllGuns;
                @DespawnAllGuns.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnDespawnAllGuns;
                @GenerateGun.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnGenerateGun;
                @GenerateGun.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnGenerateGun;
                @GenerateGun.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnGenerateGun;
                @Move.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMove;
                @Move.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMove;
                @Move.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMove;
                @Reload.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnReload;
                @Reload.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnReload;
                @Reload.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnReload;
                @Aim.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnAim;
                @Aim.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnAim;
                @Aim.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnAim;
                @Shoot.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnShoot;
                @Shoot.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnShoot;
                @Shoot.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnShoot;
                @Gun1.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnGun1;
                @Gun1.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnGun1;
                @Gun1.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnGun1;
                @Gun2.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnGun2;
                @Gun2.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnGun2;
                @Gun2.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnGun2;
                @Gun3.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnGun3;
                @Gun3.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnGun3;
                @Gun3.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnGun3;
                @WeaponWheel.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnWeaponWheel;
                @WeaponWheel.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnWeaponWheel;
                @WeaponWheel.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnWeaponWheel;
            }
            m_Wrapper.m_PlayerActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Movement.started += instance.OnMovement;
                @Movement.performed += instance.OnMovement;
                @Movement.canceled += instance.OnMovement;
                @Jump.started += instance.OnJump;
                @Jump.performed += instance.OnJump;
                @Jump.canceled += instance.OnJump;
                @Look.started += instance.OnLook;
                @Look.performed += instance.OnLook;
                @Look.canceled += instance.OnLook;
                @Dash.started += instance.OnDash;
                @Dash.performed += instance.OnDash;
                @Dash.canceled += instance.OnDash;
                @TestButton2.started += instance.OnTestButton2;
                @TestButton2.performed += instance.OnTestButton2;
                @TestButton2.canceled += instance.OnTestButton2;
                @TestButton1.started += instance.OnTestButton1;
                @TestButton1.performed += instance.OnTestButton1;
                @TestButton1.canceled += instance.OnTestButton1;
                @DespawnAllGuns.started += instance.OnDespawnAllGuns;
                @DespawnAllGuns.performed += instance.OnDespawnAllGuns;
                @DespawnAllGuns.canceled += instance.OnDespawnAllGuns;
                @GenerateGun.started += instance.OnGenerateGun;
                @GenerateGun.performed += instance.OnGenerateGun;
                @GenerateGun.canceled += instance.OnGenerateGun;
                @Move.started += instance.OnMove;
                @Move.performed += instance.OnMove;
                @Move.canceled += instance.OnMove;
                @Reload.started += instance.OnReload;
                @Reload.performed += instance.OnReload;
                @Reload.canceled += instance.OnReload;
                @Aim.started += instance.OnAim;
                @Aim.performed += instance.OnAim;
                @Aim.canceled += instance.OnAim;
                @Shoot.started += instance.OnShoot;
                @Shoot.performed += instance.OnShoot;
                @Shoot.canceled += instance.OnShoot;
                @Gun1.started += instance.OnGun1;
                @Gun1.performed += instance.OnGun1;
                @Gun1.canceled += instance.OnGun1;
                @Gun2.started += instance.OnGun2;
                @Gun2.performed += instance.OnGun2;
                @Gun2.canceled += instance.OnGun2;
                @Gun3.started += instance.OnGun3;
                @Gun3.performed += instance.OnGun3;
                @Gun3.canceled += instance.OnGun3;
                @WeaponWheel.started += instance.OnWeaponWheel;
                @WeaponWheel.performed += instance.OnWeaponWheel;
                @WeaponWheel.canceled += instance.OnWeaponWheel;
            }
        }
    }
    public PlayerActions @Player => new PlayerActions(this);
    private int m_MKSchemeIndex = -1;
    public InputControlScheme MKScheme
    {
        get
        {
            if (m_MKSchemeIndex == -1) m_MKSchemeIndex = asset.FindControlSchemeIndex("M&K");
            return asset.controlSchemes[m_MKSchemeIndex];
        }
    }
    public interface IPlayerActions
    {
        void OnMovement(InputAction.CallbackContext context);
        void OnJump(InputAction.CallbackContext context);
        void OnLook(InputAction.CallbackContext context);
        void OnDash(InputAction.CallbackContext context);
        void OnTestButton2(InputAction.CallbackContext context);
        void OnTestButton1(InputAction.CallbackContext context);
        void OnDespawnAllGuns(InputAction.CallbackContext context);
        void OnGenerateGun(InputAction.CallbackContext context);
        void OnMove(InputAction.CallbackContext context);
        void OnReload(InputAction.CallbackContext context);
        void OnAim(InputAction.CallbackContext context);
        void OnShoot(InputAction.CallbackContext context);
        void OnGun1(InputAction.CallbackContext context);
        void OnGun2(InputAction.CallbackContext context);
        void OnGun3(InputAction.CallbackContext context);
        void OnWeaponWheel(InputAction.CallbackContext context);
    }
}

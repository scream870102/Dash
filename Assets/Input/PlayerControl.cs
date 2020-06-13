// GENERATED AUTOMATICALLY FROM 'Assets/Input/PlayerControl.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

namespace CJStudio.Dash
{
    public class @PlayerControl : IInputActionCollection, IDisposable
    {
        public InputActionAsset asset { get; }
        public @PlayerControl()
        {
            asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerControl"",
    ""maps"": [
        {
            ""name"": ""GamePlay"",
            ""id"": ""7089f7e1-6956-47e0-8e65-009ba473cad9"",
            ""actions"": [
                {
                    ""name"": ""Move"",
                    ""type"": ""Value"",
                    ""id"": ""48ae58bb-a148-46ea-a1dc-016298a9103c"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Aim"",
                    ""type"": ""Value"",
                    ""id"": ""16ed0fe0-31ac-4cf0-8dc4-9973ba4fb39e"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Jump"",
                    ""type"": ""Button"",
                    ""id"": ""684986ca-c1ea-4fa0-a44a-581ee49ee5d8"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Dash"",
                    ""type"": ""Button"",
                    ""id"": ""6ed0a2bf-5afe-4a0a-8047-3473ab6f600c"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Option"",
                    ""type"": ""Button"",
                    ""id"": ""5738e575-e74f-4782-8312-255186ca494e"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""2D Vector"",
                    ""id"": ""bab8dd49-c1b0-41be-8a37-20e0627e0cf9"",
                    ""path"": ""2DVector(normalize=false)"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""3e1977ac-7447-4021-9205-fc87525a791c"",
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
                    ""id"": ""6bf873df-e3c0-4ec6-8f6f-c252b9ed7d00"",
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
                    ""id"": ""39cc8f17-f50f-439a-8276-d3fdc44f8207"",
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
                    ""id"": ""cdfbf743-1e9e-422c-b072-0cd8f0708a72"",
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
                    ""id"": ""66d1cd25-fd6a-449b-a581-f39bd9ac281a"",
                    ""path"": ""<Gamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e78ca156-cab8-4c0b-a207-ac24f648e1f0"",
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
                    ""id"": ""48505670-d1f2-48f8-9354-d67774276c96"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""fb8e1e39-7f4c-4716-9ce8-b56371b4eafa"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""490ad6b3-96be-4237-b945-4971099cba31"",
                    ""path"": ""<Gamepad>/rightStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Aim"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""735aeb42-5a6f-4b1e-bd67-2388bff42ce0"",
                    ""path"": ""<Gamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Aim"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f45a3532-864b-4cf4-925f-81f438fc61cc"",
                    ""path"": ""<Mouse>/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Aim"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""acbfdfd9-9e6d-4e44-8f8e-d26325f4fe86"",
                    ""path"": ""<Keyboard>/leftShift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Dash"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""8f265a38-52d1-41dd-91b1-0c0893449ee9"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Dash"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""4e324346-04d1-48f8-b637-d26b203d734b"",
                    ""path"": ""<Gamepad>/leftTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Dash"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""38002041-94f2-43be-b1c1-e607b616c0a0"",
                    ""path"": ""<Gamepad>/leftShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Dash"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""bb48d939-8ac1-4e57-b5d6-bff89d15a7c6"",
                    ""path"": ""<Gamepad>/start"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Option"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""49a581a2-9ec1-4a7f-b598-ee7035e086e5"",
                    ""path"": ""<Keyboard>/enter"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Option"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b8380e83-899f-4e95-a5ce-537bbaaac577"",
                    ""path"": ""<Gamepad>/rightShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Dash"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""7ce09a85-d2d3-4099-84ec-a6fc0342cc96"",
                    ""path"": ""<Gamepad>/rightTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Dash"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""UI"",
            ""id"": ""ef56a287-1935-43c5-b1d1-c47020e1ff91"",
            ""actions"": [
                {
                    ""name"": ""Choose"",
                    ""type"": ""Value"",
                    ""id"": ""34c944aa-1432-4456-85df-93f52d63da46"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Cancel"",
                    ""type"": ""Button"",
                    ""id"": ""3bfaa53d-9202-4187-80b1-9d58fb25d757"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Confirm"",
                    ""type"": ""Button"",
                    ""id"": ""3691cb56-5925-425f-b518-03ef67e0d549"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Option"",
                    ""type"": ""Button"",
                    ""id"": ""a2996a83-a08e-4ad7-8d75-2ef469718001"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""4a3e0a8a-8782-41dc-8c1a-d156873344bc"",
                    ""path"": ""<Gamepad>/dpad/y"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Choose"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e20565ed-4743-45c9-ade4-fb05c5429bc0"",
                    ""path"": ""<Gamepad>/leftStick/y"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Choose"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""1D Axis"",
                    ""id"": ""ee90b619-cf3a-4024-bb2d-b6f58ca525c0"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Choose"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""5672fd47-fe48-4fb3-a9f5-0eb13f090e69"",
                    ""path"": ""<Keyboard>/downArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Choose"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""df7c7a8a-d76c-46b4-827e-da8056e699f2"",
                    ""path"": ""<Keyboard>/upArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Choose"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""51f3ec2b-d9e0-4cad-8dbe-2b688812c40c"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Cancel"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e5ef6e29-eafe-4dd5-8ba1-51fb2e8d5c47"",
                    ""path"": ""<Keyboard>/backspace"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Cancel"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""45826af3-5f7c-4bcb-8a05-4a7845f515df"",
                    ""path"": ""<Gamepad>/buttonEast"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Confirm"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""4c332517-2c48-4f4b-8082-f3ee12020781"",
                    ""path"": ""<Keyboard>/enter"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Confirm"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b330d845-7e7b-4ccf-8984-1047ac837c44"",
                    ""path"": ""<Gamepad>/start"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Option"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""502369b4-ac86-4ae0-8e93-91d7bde79e48"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Option"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
            // GamePlay
            m_GamePlay = asset.FindActionMap("GamePlay", throwIfNotFound: true);
            m_GamePlay_Move = m_GamePlay.FindAction("Move", throwIfNotFound: true);
            m_GamePlay_Aim = m_GamePlay.FindAction("Aim", throwIfNotFound: true);
            m_GamePlay_Jump = m_GamePlay.FindAction("Jump", throwIfNotFound: true);
            m_GamePlay_Dash = m_GamePlay.FindAction("Dash", throwIfNotFound: true);
            m_GamePlay_Option = m_GamePlay.FindAction("Option", throwIfNotFound: true);
            // UI
            m_UI = asset.FindActionMap("UI", throwIfNotFound: true);
            m_UI_Choose = m_UI.FindAction("Choose", throwIfNotFound: true);
            m_UI_Cancel = m_UI.FindAction("Cancel", throwIfNotFound: true);
            m_UI_Confirm = m_UI.FindAction("Confirm", throwIfNotFound: true);
            m_UI_Option = m_UI.FindAction("Option", throwIfNotFound: true);
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

        // GamePlay
        private readonly InputActionMap m_GamePlay;
        private IGamePlayActions m_GamePlayActionsCallbackInterface;
        private readonly InputAction m_GamePlay_Move;
        private readonly InputAction m_GamePlay_Aim;
        private readonly InputAction m_GamePlay_Jump;
        private readonly InputAction m_GamePlay_Dash;
        private readonly InputAction m_GamePlay_Option;
        public struct GamePlayActions
        {
            private @PlayerControl m_Wrapper;
            public GamePlayActions(@PlayerControl wrapper) { m_Wrapper = wrapper; }
            public InputAction @Move => m_Wrapper.m_GamePlay_Move;
            public InputAction @Aim => m_Wrapper.m_GamePlay_Aim;
            public InputAction @Jump => m_Wrapper.m_GamePlay_Jump;
            public InputAction @Dash => m_Wrapper.m_GamePlay_Dash;
            public InputAction @Option => m_Wrapper.m_GamePlay_Option;
            public InputActionMap Get() { return m_Wrapper.m_GamePlay; }
            public void Enable() { Get().Enable(); }
            public void Disable() { Get().Disable(); }
            public bool enabled => Get().enabled;
            public static implicit operator InputActionMap(GamePlayActions set) { return set.Get(); }
            public void SetCallbacks(IGamePlayActions instance)
            {
                if (m_Wrapper.m_GamePlayActionsCallbackInterface != null)
                {
                    @Move.started -= m_Wrapper.m_GamePlayActionsCallbackInterface.OnMove;
                    @Move.performed -= m_Wrapper.m_GamePlayActionsCallbackInterface.OnMove;
                    @Move.canceled -= m_Wrapper.m_GamePlayActionsCallbackInterface.OnMove;
                    @Aim.started -= m_Wrapper.m_GamePlayActionsCallbackInterface.OnAim;
                    @Aim.performed -= m_Wrapper.m_GamePlayActionsCallbackInterface.OnAim;
                    @Aim.canceled -= m_Wrapper.m_GamePlayActionsCallbackInterface.OnAim;
                    @Jump.started -= m_Wrapper.m_GamePlayActionsCallbackInterface.OnJump;
                    @Jump.performed -= m_Wrapper.m_GamePlayActionsCallbackInterface.OnJump;
                    @Jump.canceled -= m_Wrapper.m_GamePlayActionsCallbackInterface.OnJump;
                    @Dash.started -= m_Wrapper.m_GamePlayActionsCallbackInterface.OnDash;
                    @Dash.performed -= m_Wrapper.m_GamePlayActionsCallbackInterface.OnDash;
                    @Dash.canceled -= m_Wrapper.m_GamePlayActionsCallbackInterface.OnDash;
                    @Option.started -= m_Wrapper.m_GamePlayActionsCallbackInterface.OnOption;
                    @Option.performed -= m_Wrapper.m_GamePlayActionsCallbackInterface.OnOption;
                    @Option.canceled -= m_Wrapper.m_GamePlayActionsCallbackInterface.OnOption;
                }
                m_Wrapper.m_GamePlayActionsCallbackInterface = instance;
                if (instance != null)
                {
                    @Move.started += instance.OnMove;
                    @Move.performed += instance.OnMove;
                    @Move.canceled += instance.OnMove;
                    @Aim.started += instance.OnAim;
                    @Aim.performed += instance.OnAim;
                    @Aim.canceled += instance.OnAim;
                    @Jump.started += instance.OnJump;
                    @Jump.performed += instance.OnJump;
                    @Jump.canceled += instance.OnJump;
                    @Dash.started += instance.OnDash;
                    @Dash.performed += instance.OnDash;
                    @Dash.canceled += instance.OnDash;
                    @Option.started += instance.OnOption;
                    @Option.performed += instance.OnOption;
                    @Option.canceled += instance.OnOption;
                }
            }
        }
        public GamePlayActions @GamePlay => new GamePlayActions(this);

        // UI
        private readonly InputActionMap m_UI;
        private IUIActions m_UIActionsCallbackInterface;
        private readonly InputAction m_UI_Choose;
        private readonly InputAction m_UI_Cancel;
        private readonly InputAction m_UI_Confirm;
        private readonly InputAction m_UI_Option;
        public struct UIActions
        {
            private @PlayerControl m_Wrapper;
            public UIActions(@PlayerControl wrapper) { m_Wrapper = wrapper; }
            public InputAction @Choose => m_Wrapper.m_UI_Choose;
            public InputAction @Cancel => m_Wrapper.m_UI_Cancel;
            public InputAction @Confirm => m_Wrapper.m_UI_Confirm;
            public InputAction @Option => m_Wrapper.m_UI_Option;
            public InputActionMap Get() { return m_Wrapper.m_UI; }
            public void Enable() { Get().Enable(); }
            public void Disable() { Get().Disable(); }
            public bool enabled => Get().enabled;
            public static implicit operator InputActionMap(UIActions set) { return set.Get(); }
            public void SetCallbacks(IUIActions instance)
            {
                if (m_Wrapper.m_UIActionsCallbackInterface != null)
                {
                    @Choose.started -= m_Wrapper.m_UIActionsCallbackInterface.OnChoose;
                    @Choose.performed -= m_Wrapper.m_UIActionsCallbackInterface.OnChoose;
                    @Choose.canceled -= m_Wrapper.m_UIActionsCallbackInterface.OnChoose;
                    @Cancel.started -= m_Wrapper.m_UIActionsCallbackInterface.OnCancel;
                    @Cancel.performed -= m_Wrapper.m_UIActionsCallbackInterface.OnCancel;
                    @Cancel.canceled -= m_Wrapper.m_UIActionsCallbackInterface.OnCancel;
                    @Confirm.started -= m_Wrapper.m_UIActionsCallbackInterface.OnConfirm;
                    @Confirm.performed -= m_Wrapper.m_UIActionsCallbackInterface.OnConfirm;
                    @Confirm.canceled -= m_Wrapper.m_UIActionsCallbackInterface.OnConfirm;
                    @Option.started -= m_Wrapper.m_UIActionsCallbackInterface.OnOption;
                    @Option.performed -= m_Wrapper.m_UIActionsCallbackInterface.OnOption;
                    @Option.canceled -= m_Wrapper.m_UIActionsCallbackInterface.OnOption;
                }
                m_Wrapper.m_UIActionsCallbackInterface = instance;
                if (instance != null)
                {
                    @Choose.started += instance.OnChoose;
                    @Choose.performed += instance.OnChoose;
                    @Choose.canceled += instance.OnChoose;
                    @Cancel.started += instance.OnCancel;
                    @Cancel.performed += instance.OnCancel;
                    @Cancel.canceled += instance.OnCancel;
                    @Confirm.started += instance.OnConfirm;
                    @Confirm.performed += instance.OnConfirm;
                    @Confirm.canceled += instance.OnConfirm;
                    @Option.started += instance.OnOption;
                    @Option.performed += instance.OnOption;
                    @Option.canceled += instance.OnOption;
                }
            }
        }
        public UIActions @UI => new UIActions(this);
        public interface IGamePlayActions
        {
            void OnMove(InputAction.CallbackContext context);
            void OnAim(InputAction.CallbackContext context);
            void OnJump(InputAction.CallbackContext context);
            void OnDash(InputAction.CallbackContext context);
            void OnOption(InputAction.CallbackContext context);
        }
        public interface IUIActions
        {
            void OnChoose(InputAction.CallbackContext context);
            void OnCancel(InputAction.CallbackContext context);
            void OnConfirm(InputAction.CallbackContext context);
            void OnOption(InputAction.CallbackContext context);
        }
    }
}

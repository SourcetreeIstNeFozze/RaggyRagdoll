// GENERATED AUTOMATICALLY FROM 'Assets/Input/InputController.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @InputController : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @InputController()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""InputController"",
    ""maps"": [
        {
            ""name"": ""HandMovement"",
            ""id"": ""6e7a5973-8e43-4caf-b66b-aa42cd0f7bac"",
            ""actions"": [
                {
                    ""name"": ""IndexFingerUP"",
                    ""type"": ""Button"",
                    ""id"": ""b5cf07c2-5eda-452b-a676-eef5a05465d8"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press""
                },
                {
                    ""name"": ""IndexFingerDOWN"",
                    ""type"": ""Button"",
                    ""id"": ""c81dff1e-d853-4317-a6c5-5053376bd106"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press(behavior=1)""
                },
                {
                    ""name"": ""IndexFingerCurledIN"",
                    ""type"": ""Button"",
                    ""id"": ""f22fc432-7836-46fe-b65d-0f5d25c7a771"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press""
                },
                {
                    ""name"": ""IndexFingerCurledOUT"",
                    ""type"": ""Button"",
                    ""id"": ""bd16f9bb-438d-44a7-93ee-10c8e262397c"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press(behavior=1)""
                },
                {
                    ""name"": ""MiddleFingerUP"",
                    ""type"": ""Button"",
                    ""id"": ""4aa8578e-3c78-4bda-b8cf-4984362f26d1"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press""
                },
                {
                    ""name"": ""MiddleFingerDOWN"",
                    ""type"": ""Button"",
                    ""id"": ""eda7615c-542c-4374-a2ea-886d7e6df38a"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press(behavior=1)""
                },
                {
                    ""name"": ""MiddleFingerCurledIN"",
                    ""type"": ""Button"",
                    ""id"": ""58550246-fa03-4917-8eec-1657cba2016e"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press""
                },
                {
                    ""name"": ""MiddleFingerCurledOUT"",
                    ""type"": ""Button"",
                    ""id"": ""6bdf8d78-9951-44a3-bb81-0104b5fe4e0e"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press(behavior=1)""
                },
                {
                    ""name"": ""StandUp"",
                    ""type"": ""Button"",
                    ""id"": ""d00aa6d5-c5d1-4c1c-8cb2-aeb5c4ca19df"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press""
                },
                {
                    ""name"": ""BodyBending"",
                    ""type"": ""Value"",
                    ""id"": ""40fc7a63-6074-40ea-be88-ea9046ee0dc6"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""90caa107-9989-4d41-9988-4f7ec216e974"",
                    ""path"": ""<Gamepad>/leftShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""IndexFingerUP"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""83e115b2-fbd3-486b-b92c-f9f059cd6d67"",
                    ""path"": ""<Keyboard>/q"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardScheme1"",
                    ""action"": ""IndexFingerUP"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""2c22e05a-1c96-4946-b6d0-76e043f74d8d"",
                    ""path"": ""<Keyboard>/8"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardScheme2;KeyboardScheme1"",
                    ""action"": ""IndexFingerUP"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b8afe06c-bc04-4178-8563-274df84d1d6f"",
                    ""path"": ""<Gamepad>/rightTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MiddleFingerCurledIN"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d78fca9d-d955-40fa-a88a-e392ae54177b"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardScheme1"",
                    ""action"": ""MiddleFingerCurledIN"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""4d1556ae-8765-4cd5-944c-b96b945c25cb"",
                    ""path"": ""<Keyboard>/4"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardScheme2"",
                    ""action"": ""MiddleFingerCurledIN"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""bfea6db8-08c1-4a19-aa43-49e360f3b84a"",
                    ""path"": ""<Gamepad>/leftShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""IndexFingerDOWN"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""ec39b1d2-495f-4cff-a032-db2891f0af6e"",
                    ""path"": ""<Keyboard>/q"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardScheme1"",
                    ""action"": ""IndexFingerDOWN"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""ce1dcf7e-67cd-4976-9614-a91ecd883916"",
                    ""path"": ""<Keyboard>/8"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardScheme2"",
                    ""action"": ""IndexFingerDOWN"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""0b6b170b-f148-4350-bea8-cd2289694a4a"",
                    ""path"": ""<Gamepad>/buttonEast"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""StandUp"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a7f2439c-982b-4b85-9c47-855daee494a9"",
                    ""path"": ""<Keyboard>/leftShift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardScheme1"",
                    ""action"": ""StandUp"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""ade8ea5e-06cb-479e-9bc4-054c31a63713"",
                    ""path"": ""<Keyboard>/numpadEnter"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""StandUp"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""7e9186ce-6caf-4e38-86fb-17d945c515ef"",
                    ""path"": ""<Gamepad>/leftTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""IndexFingerCurledIN"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""ce91849d-6fcd-4740-b3a6-73cadc286aed"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardScheme1"",
                    ""action"": ""IndexFingerCurledIN"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""5fb172ee-8734-4306-972b-de5ded0d52a3"",
                    ""path"": ""<Keyboard>/5"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardScheme2"",
                    ""action"": ""IndexFingerCurledIN"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c18cceab-a851-4d3c-827c-2b0ab5ef3b92"",
                    ""path"": ""<Gamepad>/leftTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""IndexFingerCurledOUT"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""cc9300b3-91d0-42f0-a336-d13951bdeec5"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardScheme1"",
                    ""action"": ""IndexFingerCurledOUT"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""9ce4cebd-7c6a-4505-b3ac-4a66228c9036"",
                    ""path"": ""<Keyboard>/5"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardScheme2"",
                    ""action"": ""IndexFingerCurledOUT"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""803754cb-da3b-44cb-82f9-0ca4abf0e94d"",
                    ""path"": ""<Gamepad>/rightShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MiddleFingerUP"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""8abf6c58-2776-4cfa-ad44-d30d685da8c5"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardScheme1"",
                    ""action"": ""MiddleFingerUP"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""924eda7a-439a-4e97-b209-4a8527ec5ea6"",
                    ""path"": ""<Keyboard>/7"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardScheme2"",
                    ""action"": ""MiddleFingerUP"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""76cab51d-8438-41d6-bba0-8e74099fe4bb"",
                    ""path"": ""<Gamepad>/rightShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MiddleFingerDOWN"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""193ff8cc-60af-4563-bf37-ba39055321dd"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardScheme1"",
                    ""action"": ""MiddleFingerDOWN"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b6c47b55-c916-43bc-a9dd-af9b14b3de0c"",
                    ""path"": ""<Keyboard>/7"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardScheme2"",
                    ""action"": ""MiddleFingerDOWN"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""ca83dba5-2f98-45a6-ae3e-605a23781021"",
                    ""path"": ""<Gamepad>/rightTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MiddleFingerCurledOUT"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""56fdf35b-27b4-4828-82be-824f28800e31"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardScheme1"",
                    ""action"": ""MiddleFingerCurledOUT"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""fb42b5fa-3a74-463b-b7e2-09f927b3ec98"",
                    ""path"": ""<Keyboard>/4"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardScheme2"",
                    ""action"": ""MiddleFingerCurledOUT"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""2D Vector"",
                    ""id"": ""762dbf19-5953-4148-b59f-c1bc866a5e3b"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""BodyBending"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""2df6ddce-33d7-4e2e-a006-6dbee9f0a897"",
                    ""path"": ""<Gamepad>/leftStick/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""BodyBending"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""6975f733-f752-453a-98f6-04f8590bd11f"",
                    ""path"": ""<Gamepad>/leftStick/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""BodyBending"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""5f4edb10-b665-4386-9332-5f3373021742"",
                    ""path"": ""<Gamepad>/leftStick/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""BodyBending"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""9d6e71cd-feb7-48b8-a74e-7594e311b00b"",
                    ""path"": ""<Gamepad>/leftStick/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""BodyBending"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""2D Vector"",
                    ""id"": ""56d9ac54-32c3-4872-8353-3d266d9acea4"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""BodyBending"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""b04366dc-6941-49e3-9e69-f5002781b569"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardScheme1"",
                    ""action"": ""BodyBending"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""48706e48-477b-4a4b-8f88-5f8036ff55dd"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardScheme1"",
                    ""action"": ""BodyBending"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""4af69423-3c39-4276-a707-590b9188e9b6"",
                    ""path"": ""<Keyboard>/z"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardScheme1"",
                    ""action"": ""BodyBending"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""caa838f6-1d48-4ef4-adde-55ad8ca4f5ba"",
                    ""path"": ""<Keyboard>/x"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardScheme1;KeyboardScheme2"",
                    ""action"": ""BodyBending"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""2D Vector"",
                    ""id"": ""f6765106-bf88-4a1a-b9be-833f1f31f71f"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""BodyBending"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""3c126196-60a2-445f-b860-e28905d71454"",
                    ""path"": ""<Keyboard>/9"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardScheme2"",
                    ""action"": ""BodyBending"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""43c31b50-e162-4139-8e3a-724be1f03bc0"",
                    ""path"": ""<Keyboard>/6"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardScheme2"",
                    ""action"": ""BodyBending"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""f7559620-4709-4c8a-bce1-91ff1dc3aab5"",
                    ""path"": ""<Keyboard>/1"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardScheme2"",
                    ""action"": ""BodyBending"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""fa742ed4-5af4-45d8-a190-7a318b897eeb"",
                    ""path"": ""<Keyboard>/2"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardScheme2"",
                    ""action"": ""BodyBending"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                }
            ]
        },
        {
            ""name"": ""Menus"",
            ""id"": ""85e84938-2951-4e17-9008-f2b4f63a9477"",
            ""actions"": [
                {
                    ""name"": ""New action"",
                    ""type"": ""Button"",
                    ""id"": ""ed8eeb0a-6503-49f6-bdb4-bb05af7a6d19"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""69ec047a-15cd-4160-9ee9-4e8494a15e88"",
                    ""path"": """",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""New action"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""Gamepad"",
            ""bindingGroup"": ""Gamepad"",
            ""devices"": [
                {
                    ""devicePath"": ""<Gamepad>"",
                    ""isOptional"": true,
                    ""isOR"": false
                }
            ]
        },
        {
            ""name"": ""KeyboardScheme2"",
            ""bindingGroup"": ""KeyboardScheme2"",
            ""devices"": []
        },
        {
            ""name"": ""KeyboardScheme1"",
            ""bindingGroup"": ""KeyboardScheme1"",
            ""devices"": [
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": true,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
        // HandMovement
        m_HandMovement = asset.FindActionMap("HandMovement", throwIfNotFound: true);
        m_HandMovement_IndexFingerUP = m_HandMovement.FindAction("IndexFingerUP", throwIfNotFound: true);
        m_HandMovement_IndexFingerDOWN = m_HandMovement.FindAction("IndexFingerDOWN", throwIfNotFound: true);
        m_HandMovement_IndexFingerCurledIN = m_HandMovement.FindAction("IndexFingerCurledIN", throwIfNotFound: true);
        m_HandMovement_IndexFingerCurledOUT = m_HandMovement.FindAction("IndexFingerCurledOUT", throwIfNotFound: true);
        m_HandMovement_MiddleFingerUP = m_HandMovement.FindAction("MiddleFingerUP", throwIfNotFound: true);
        m_HandMovement_MiddleFingerDOWN = m_HandMovement.FindAction("MiddleFingerDOWN", throwIfNotFound: true);
        m_HandMovement_MiddleFingerCurledIN = m_HandMovement.FindAction("MiddleFingerCurledIN", throwIfNotFound: true);
        m_HandMovement_MiddleFingerCurledOUT = m_HandMovement.FindAction("MiddleFingerCurledOUT", throwIfNotFound: true);
        m_HandMovement_StandUp = m_HandMovement.FindAction("StandUp", throwIfNotFound: true);
        m_HandMovement_BodyBending = m_HandMovement.FindAction("BodyBending", throwIfNotFound: true);
        // Menus
        m_Menus = asset.FindActionMap("Menus", throwIfNotFound: true);
        m_Menus_Newaction = m_Menus.FindAction("New action", throwIfNotFound: true);
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

    // HandMovement
    private readonly InputActionMap m_HandMovement;
    private IHandMovementActions m_HandMovementActionsCallbackInterface;
    private readonly InputAction m_HandMovement_IndexFingerUP;
    private readonly InputAction m_HandMovement_IndexFingerDOWN;
    private readonly InputAction m_HandMovement_IndexFingerCurledIN;
    private readonly InputAction m_HandMovement_IndexFingerCurledOUT;
    private readonly InputAction m_HandMovement_MiddleFingerUP;
    private readonly InputAction m_HandMovement_MiddleFingerDOWN;
    private readonly InputAction m_HandMovement_MiddleFingerCurledIN;
    private readonly InputAction m_HandMovement_MiddleFingerCurledOUT;
    private readonly InputAction m_HandMovement_StandUp;
    private readonly InputAction m_HandMovement_BodyBending;
    public struct HandMovementActions
    {
        private @InputController m_Wrapper;
        public HandMovementActions(@InputController wrapper) { m_Wrapper = wrapper; }
        public InputAction @IndexFingerUP => m_Wrapper.m_HandMovement_IndexFingerUP;
        public InputAction @IndexFingerDOWN => m_Wrapper.m_HandMovement_IndexFingerDOWN;
        public InputAction @IndexFingerCurledIN => m_Wrapper.m_HandMovement_IndexFingerCurledIN;
        public InputAction @IndexFingerCurledOUT => m_Wrapper.m_HandMovement_IndexFingerCurledOUT;
        public InputAction @MiddleFingerUP => m_Wrapper.m_HandMovement_MiddleFingerUP;
        public InputAction @MiddleFingerDOWN => m_Wrapper.m_HandMovement_MiddleFingerDOWN;
        public InputAction @MiddleFingerCurledIN => m_Wrapper.m_HandMovement_MiddleFingerCurledIN;
        public InputAction @MiddleFingerCurledOUT => m_Wrapper.m_HandMovement_MiddleFingerCurledOUT;
        public InputAction @StandUp => m_Wrapper.m_HandMovement_StandUp;
        public InputAction @BodyBending => m_Wrapper.m_HandMovement_BodyBending;
        public InputActionMap Get() { return m_Wrapper.m_HandMovement; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(HandMovementActions set) { return set.Get(); }
        public void SetCallbacks(IHandMovementActions instance)
        {
            if (m_Wrapper.m_HandMovementActionsCallbackInterface != null)
            {
                @IndexFingerUP.started -= m_Wrapper.m_HandMovementActionsCallbackInterface.OnIndexFingerUP;
                @IndexFingerUP.performed -= m_Wrapper.m_HandMovementActionsCallbackInterface.OnIndexFingerUP;
                @IndexFingerUP.canceled -= m_Wrapper.m_HandMovementActionsCallbackInterface.OnIndexFingerUP;
                @IndexFingerDOWN.started -= m_Wrapper.m_HandMovementActionsCallbackInterface.OnIndexFingerDOWN;
                @IndexFingerDOWN.performed -= m_Wrapper.m_HandMovementActionsCallbackInterface.OnIndexFingerDOWN;
                @IndexFingerDOWN.canceled -= m_Wrapper.m_HandMovementActionsCallbackInterface.OnIndexFingerDOWN;
                @IndexFingerCurledIN.started -= m_Wrapper.m_HandMovementActionsCallbackInterface.OnIndexFingerCurledIN;
                @IndexFingerCurledIN.performed -= m_Wrapper.m_HandMovementActionsCallbackInterface.OnIndexFingerCurledIN;
                @IndexFingerCurledIN.canceled -= m_Wrapper.m_HandMovementActionsCallbackInterface.OnIndexFingerCurledIN;
                @IndexFingerCurledOUT.started -= m_Wrapper.m_HandMovementActionsCallbackInterface.OnIndexFingerCurledOUT;
                @IndexFingerCurledOUT.performed -= m_Wrapper.m_HandMovementActionsCallbackInterface.OnIndexFingerCurledOUT;
                @IndexFingerCurledOUT.canceled -= m_Wrapper.m_HandMovementActionsCallbackInterface.OnIndexFingerCurledOUT;
                @MiddleFingerUP.started -= m_Wrapper.m_HandMovementActionsCallbackInterface.OnMiddleFingerUP;
                @MiddleFingerUP.performed -= m_Wrapper.m_HandMovementActionsCallbackInterface.OnMiddleFingerUP;
                @MiddleFingerUP.canceled -= m_Wrapper.m_HandMovementActionsCallbackInterface.OnMiddleFingerUP;
                @MiddleFingerDOWN.started -= m_Wrapper.m_HandMovementActionsCallbackInterface.OnMiddleFingerDOWN;
                @MiddleFingerDOWN.performed -= m_Wrapper.m_HandMovementActionsCallbackInterface.OnMiddleFingerDOWN;
                @MiddleFingerDOWN.canceled -= m_Wrapper.m_HandMovementActionsCallbackInterface.OnMiddleFingerDOWN;
                @MiddleFingerCurledIN.started -= m_Wrapper.m_HandMovementActionsCallbackInterface.OnMiddleFingerCurledIN;
                @MiddleFingerCurledIN.performed -= m_Wrapper.m_HandMovementActionsCallbackInterface.OnMiddleFingerCurledIN;
                @MiddleFingerCurledIN.canceled -= m_Wrapper.m_HandMovementActionsCallbackInterface.OnMiddleFingerCurledIN;
                @MiddleFingerCurledOUT.started -= m_Wrapper.m_HandMovementActionsCallbackInterface.OnMiddleFingerCurledOUT;
                @MiddleFingerCurledOUT.performed -= m_Wrapper.m_HandMovementActionsCallbackInterface.OnMiddleFingerCurledOUT;
                @MiddleFingerCurledOUT.canceled -= m_Wrapper.m_HandMovementActionsCallbackInterface.OnMiddleFingerCurledOUT;
                @StandUp.started -= m_Wrapper.m_HandMovementActionsCallbackInterface.OnStandUp;
                @StandUp.performed -= m_Wrapper.m_HandMovementActionsCallbackInterface.OnStandUp;
                @StandUp.canceled -= m_Wrapper.m_HandMovementActionsCallbackInterface.OnStandUp;
                @BodyBending.started -= m_Wrapper.m_HandMovementActionsCallbackInterface.OnBodyBending;
                @BodyBending.performed -= m_Wrapper.m_HandMovementActionsCallbackInterface.OnBodyBending;
                @BodyBending.canceled -= m_Wrapper.m_HandMovementActionsCallbackInterface.OnBodyBending;
            }
            m_Wrapper.m_HandMovementActionsCallbackInterface = instance;
            if (instance != null)
            {
                @IndexFingerUP.started += instance.OnIndexFingerUP;
                @IndexFingerUP.performed += instance.OnIndexFingerUP;
                @IndexFingerUP.canceled += instance.OnIndexFingerUP;
                @IndexFingerDOWN.started += instance.OnIndexFingerDOWN;
                @IndexFingerDOWN.performed += instance.OnIndexFingerDOWN;
                @IndexFingerDOWN.canceled += instance.OnIndexFingerDOWN;
                @IndexFingerCurledIN.started += instance.OnIndexFingerCurledIN;
                @IndexFingerCurledIN.performed += instance.OnIndexFingerCurledIN;
                @IndexFingerCurledIN.canceled += instance.OnIndexFingerCurledIN;
                @IndexFingerCurledOUT.started += instance.OnIndexFingerCurledOUT;
                @IndexFingerCurledOUT.performed += instance.OnIndexFingerCurledOUT;
                @IndexFingerCurledOUT.canceled += instance.OnIndexFingerCurledOUT;
                @MiddleFingerUP.started += instance.OnMiddleFingerUP;
                @MiddleFingerUP.performed += instance.OnMiddleFingerUP;
                @MiddleFingerUP.canceled += instance.OnMiddleFingerUP;
                @MiddleFingerDOWN.started += instance.OnMiddleFingerDOWN;
                @MiddleFingerDOWN.performed += instance.OnMiddleFingerDOWN;
                @MiddleFingerDOWN.canceled += instance.OnMiddleFingerDOWN;
                @MiddleFingerCurledIN.started += instance.OnMiddleFingerCurledIN;
                @MiddleFingerCurledIN.performed += instance.OnMiddleFingerCurledIN;
                @MiddleFingerCurledIN.canceled += instance.OnMiddleFingerCurledIN;
                @MiddleFingerCurledOUT.started += instance.OnMiddleFingerCurledOUT;
                @MiddleFingerCurledOUT.performed += instance.OnMiddleFingerCurledOUT;
                @MiddleFingerCurledOUT.canceled += instance.OnMiddleFingerCurledOUT;
                @StandUp.started += instance.OnStandUp;
                @StandUp.performed += instance.OnStandUp;
                @StandUp.canceled += instance.OnStandUp;
                @BodyBending.started += instance.OnBodyBending;
                @BodyBending.performed += instance.OnBodyBending;
                @BodyBending.canceled += instance.OnBodyBending;
            }
        }
    }
    public HandMovementActions @HandMovement => new HandMovementActions(this);

    // Menus
    private readonly InputActionMap m_Menus;
    private IMenusActions m_MenusActionsCallbackInterface;
    private readonly InputAction m_Menus_Newaction;
    public struct MenusActions
    {
        private @InputController m_Wrapper;
        public MenusActions(@InputController wrapper) { m_Wrapper = wrapper; }
        public InputAction @Newaction => m_Wrapper.m_Menus_Newaction;
        public InputActionMap Get() { return m_Wrapper.m_Menus; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(MenusActions set) { return set.Get(); }
        public void SetCallbacks(IMenusActions instance)
        {
            if (m_Wrapper.m_MenusActionsCallbackInterface != null)
            {
                @Newaction.started -= m_Wrapper.m_MenusActionsCallbackInterface.OnNewaction;
                @Newaction.performed -= m_Wrapper.m_MenusActionsCallbackInterface.OnNewaction;
                @Newaction.canceled -= m_Wrapper.m_MenusActionsCallbackInterface.OnNewaction;
            }
            m_Wrapper.m_MenusActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Newaction.started += instance.OnNewaction;
                @Newaction.performed += instance.OnNewaction;
                @Newaction.canceled += instance.OnNewaction;
            }
        }
    }
    public MenusActions @Menus => new MenusActions(this);
    private int m_GamepadSchemeIndex = -1;
    public InputControlScheme GamepadScheme
    {
        get
        {
            if (m_GamepadSchemeIndex == -1) m_GamepadSchemeIndex = asset.FindControlSchemeIndex("Gamepad");
            return asset.controlSchemes[m_GamepadSchemeIndex];
        }
    }
    private int m_KeyboardScheme2SchemeIndex = -1;
    public InputControlScheme KeyboardScheme2Scheme
    {
        get
        {
            if (m_KeyboardScheme2SchemeIndex == -1) m_KeyboardScheme2SchemeIndex = asset.FindControlSchemeIndex("KeyboardScheme2");
            return asset.controlSchemes[m_KeyboardScheme2SchemeIndex];
        }
    }
    private int m_KeyboardScheme1SchemeIndex = -1;
    public InputControlScheme KeyboardScheme1Scheme
    {
        get
        {
            if (m_KeyboardScheme1SchemeIndex == -1) m_KeyboardScheme1SchemeIndex = asset.FindControlSchemeIndex("KeyboardScheme1");
            return asset.controlSchemes[m_KeyboardScheme1SchemeIndex];
        }
    }
    public interface IHandMovementActions
    {
        void OnIndexFingerUP(InputAction.CallbackContext context);
        void OnIndexFingerDOWN(InputAction.CallbackContext context);
        void OnIndexFingerCurledIN(InputAction.CallbackContext context);
        void OnIndexFingerCurledOUT(InputAction.CallbackContext context);
        void OnMiddleFingerUP(InputAction.CallbackContext context);
        void OnMiddleFingerDOWN(InputAction.CallbackContext context);
        void OnMiddleFingerCurledIN(InputAction.CallbackContext context);
        void OnMiddleFingerCurledOUT(InputAction.CallbackContext context);
        void OnStandUp(InputAction.CallbackContext context);
        void OnBodyBending(InputAction.CallbackContext context);
    }
    public interface IMenusActions
    {
        void OnNewaction(InputAction.CallbackContext context);
    }
}

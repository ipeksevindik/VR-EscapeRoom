using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Vitals
{
    public class EnemyDamager : MonoBehaviour
    {
        public GameObject Blood;
        public float damage = 10f;
        #region Input

        // The input actions
        public InputActionAsset actions;

        // The actions
        private InputAction _damageAction;
        
        private void Awake()
        {
            Blood.SetActive(false);
            // Find the actions
            _damageAction = actions.FindActionMap("EnemyDemo").FindAction("Damage");
            
            // Subscribe to the events
            _damageAction.performed += OnDamageAction;
        }

        private void OnDamageAction(InputAction.CallbackContext obj)
        {
            var mousepos = Mouse.current.position.ReadValue();
            // cast a ray from the mouse position
            var ray = Camera.main.ScreenPointToRay(mousepos);
            // check if the ray hits something
            if (!Physics.Raycast(ray, out var hitInfo)) return;
            // get the hit position
            if (hitInfo.transform.TryGetComponent(out EnemyDemo enemy))
            {
                enemy.Health.Decrease(damage);
             
                
            }
        }

        private void OnEnable()
        {
            _damageAction.Enable();
        }

        #endregion
    }
}

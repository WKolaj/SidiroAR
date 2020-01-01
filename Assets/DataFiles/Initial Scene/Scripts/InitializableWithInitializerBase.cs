using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public abstract class InitializableWithInitializerBase : MonoBehaviour
{
    /// <summary>
    /// Application initialize component
    /// </summary>
    private ApplicationInitializer _initializer;
    public ApplicationInitializer Initalizer
    {
        get
        {
            return _initializer;
        }

        private set
        {
            _initializer = value;
        }
    }

    /// <summary>
    /// Method for initializing app by initialzier
    /// </summary>
    /// <param name="initializer"></param>
    public void InitializeComponents(ApplicationInitializer initializer)
    {
        this._initializer = initializer;

        OnInitializeComponents();
    }

    /// <summary>
    /// Method called when components are being initialized via app iniitalizer
    /// </summary>
    protected abstract void OnInitializeComponents();
}

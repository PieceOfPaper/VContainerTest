using System.Collections;
using System.Collections.Generic;
using UnityEditor.iOS;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public enum UIDepthType
{
    None,
}

public abstract class IUIBasePresenter : IInitializable, IUIManagerPresenterController
{
    public abstract string PrefabName { get; }
    public abstract UIDepthType UIDepthType { get; }
    
    
    protected IUIBaseBehaviour m_Behaviour;
    public IUIBaseBehaviour Behaviour => m_Behaviour;

    protected UIManager uiManager { get; private set; }

    void IInitializable.Initialize()
    {
        uiManager.RegistUIPresenter(this);
    }
    
    [Inject]
    private void _BaseDependencyInject(UIManager uiManager)
    {
        this.uiManager = uiManager;
    }

    void IUIManagerPresenterController.CreateBehaviour(IUIBaseBehaviour behavior)
    {
        if (m_Behaviour != null)
        {
            //TODO - 무슨상황..?
        }

        m_Behaviour = behavior;
        OnCreateBehaviour();
    }
    
    protected virtual void OnCreateBehaviour() { }

    void IUIManagerPresenterController.DestroyBehaviour()
    {
        if (m_Behaviour == null)
            return;
        
        GameObject.Destroy(m_Behaviour.gameObject);
        m_Behaviour = null;
        OnDestroyBehaviour();
    }
    
    protected virtual void OnDestroyBehaviour() { }


    void IUIManagerPresenterController.OpenBehaviour()
    {
        OnOpenBehaviour();
    }
    
    protected virtual void OnOpenBehaviour() { }

    void IUIManagerPresenterController.CloseBehaviour()
    {
        OnCloseBehaviour();
    }
    
    protected virtual void OnCloseBehaviour() { }
}

public abstract class IUIBaseBehaviour : MonoBehaviour, IUIManagerBehaviourController
{
    protected IUIBasePresenter m_Presenter;
    
    void IUIManagerBehaviourController.Initialize(IUIBasePresenter presenter)
    {
        m_Presenter = presenter;
        OnInitialized();
    }

    protected virtual void OnInitialized() { }
}

public abstract class IuiBasePresenterTemplate<BT> : IUIBasePresenter where BT : IUIBaseBehaviour
{
    protected BT MyBehaviour => (BT)m_Behaviour;
}

public abstract class IuiBaseBehaviourTemplate<PT> : IUIBaseBehaviour where PT : IUIBasePresenter
{
    protected PT MyPresenter => (PT)m_Presenter;
}

public interface IUIManagerPresenterController
{
    void CreateBehaviour(IUIBaseBehaviour behavior);
    void DestroyBehaviour();

    void OpenBehaviour();
    void CloseBehaviour();
}

public interface IUIManagerBehaviourController
{
    void Initialize(IUIBasePresenter presenter);
}

public class UIManager : MonoBehaviour
{
    private Dictionary<System.Type, IUIBasePresenter> m_Presenters = new Dictionary<System.Type, IUIBasePresenter>();

    public void RegistUIPresenter(IUIBasePresenter presenter)
    {
        if (presenter == null)
            return;
        
        UnityEngine.Debug.Log($"UIManager.Regist {presenter.GetType()}");
        m_Presenters.Add(presenter.GetType(), presenter);
    }


    private void LoadUI(System.Type type)
    {
        if (m_Presenters.ContainsKey(type) == false)
        {
            return;
        }

        if (m_Presenters[type] is IUIManagerPresenterController controller)
        {
            var prefab = Resources.Load<GameObject>(m_Presenters[type].PrefabName);
            var instance = Instantiate(prefab);
            var behaviour = instance.GetComponent<IUIBaseBehaviour>();
            
            behaviour.gameObject.SetActive(false);
            controller.CreateBehaviour(behaviour);
        }
    }
    

    public void OpenUI<T>() where T : IUIBasePresenter => OpenUI(typeof(T));

    public void OpenUI(System.Type type)
    {
        if (m_Presenters.ContainsKey(type) == false)
        {
            //TODO - 에러로그
            return;
        }

        if (m_Presenters[type].Behaviour == null)
        {
            LoadUI(type);
            if (m_Presenters[type].Behaviour == null)
            {
                //TODO - 에러로그
                return;
            }
        }
        
        if (m_Presenters[type] is IUIManagerPresenterController controller)
        {
            controller.OpenBehaviour();
        }
    }
    
    public void CloseUI<T>() where T : IUIBasePresenter => CloseUI(typeof(T));

    public void CloseUI(System.Type type)
    {
        if (m_Presenters.ContainsKey(type) == false)
        {
            //TODO - 에러로그
            return;
        }

        if (m_Presenters[type].Behaviour == null)
        {
            return;
        }
        
        if (m_Presenters[type] is IUIManagerPresenterController controller)
        {
            controller.CloseBehaviour();
        }
    }
}

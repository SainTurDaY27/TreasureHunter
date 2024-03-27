using TreasureHunter.Gameplay.UI;
using TreasureHunter.Utilities;
using UnityEngine;

namespace TreasureHunter.Core.UI
{
    public class UIManager : MonoSingleton<UIManager>
    {
        
        [SerializeField] private MenuPanel _menuPanel;
        [SerializeField] private AbilitySelectionPanel _abilitySelectionPanel;
        [SerializeField] private GameHUDPanel _gameHUDPanel;
        [SerializeField] private EndGamePanel _endGamePanel;

        [SerializeField] private AbilityGetPanel _abilityGetPanel;
        [SerializeField] private TreasureGetPanel _treasureGetPanel;
        [SerializeField] private LoadGamePanel _loadGamePanel;
        [SerializeField] private MapPanel _mapPanel;
        [SerializeField] private ChooseSaveSlotPanel _chooseSaveSlotPanel;
        [SerializeField] private TutorialPanel _tutorialPanel;

        public IBaseUI Show(UIKey uiKey)
        {
            switch (uiKey)
            {
                case UIKey.Menu:
                    _menuPanel.SetActive(true);
                    return _menuPanel;
                case UIKey.AbilitySelection:
                    _abilitySelectionPanel.SetActive(true);
                    return _abilitySelectionPanel;
                case UIKey.GameHUD:
                    _gameHUDPanel.SetActive(true);
                    return _gameHUDPanel;
                case UIKey.EndGame:
                    _endGamePanel.SetActive(true);
                    return _endGamePanel;
                case UIKey.AbilityGet:
                    _abilityGetPanel.SetActive(true);
                    return _abilityGetPanel;
                case UIKey.LoadGame:
                    _loadGamePanel.SetActive(true);
                    return _loadGamePanel;
                case UIKey.TreasureGet:
                    _treasureGetPanel.SetActive(true);
                    return _treasureGetPanel;
                case UIKey.Map:
                    _mapPanel.SetActive(true);
                    return _mapPanel;
                case UIKey.ChooseSaveSlot:
                    _chooseSaveSlotPanel.SetActive(true);
                    return _chooseSaveSlotPanel;
                case UIKey.Tutorial:
                    _tutorialPanel.SetActive(true);
                    return _tutorialPanel;
                default:
                    return null;
            }
        }

        public IBaseUI Hide(UIKey uiKey)
        {
            switch (uiKey)
            {
                case UIKey.Menu:
                    _menuPanel.SetActive(false);
                    _menuPanel.transform.SetAsLastSibling();
                    return _menuPanel;
                case UIKey.AbilitySelection:
                    _abilitySelectionPanel.SetActive(false);
                    _abilitySelectionPanel.transform.SetAsLastSibling();
                    return _abilitySelectionPanel;
                case UIKey.GameHUD:
                    _gameHUDPanel.SetActive(false);
                    _gameHUDPanel.transform.SetAsLastSibling();
                    return _gameHUDPanel;
                case UIKey.EndGame:
                    _endGamePanel.SetActive(false);
                    _endGamePanel.transform.SetAsLastSibling();
                    return _endGamePanel;
                case UIKey.AbilityGet:
                    _abilityGetPanel.SetActive(false);
                    _abilityGetPanel.transform.SetAsLastSibling();
                    return _abilityGetPanel;
                case UIKey.LoadGame:
                    _loadGamePanel.SetActive(false);
                    _loadGamePanel.transform.SetAsLastSibling();
                    return _loadGamePanel;
                case UIKey.TreasureGet:
                    _treasureGetPanel.SetActive(false);
                    _treasureGetPanel.transform.SetAsLastSibling();
                    return _treasureGetPanel;
                case UIKey.Map:
                    _mapPanel.SetActive(false);
                    _mapPanel.transform.SetAsLastSibling();
                    return _mapPanel;
                case UIKey.ChooseSaveSlot:
                    _chooseSaveSlotPanel.SetActive(false);
                    _chooseSaveSlotPanel.transform.SetAsLastSibling();
                    return _chooseSaveSlotPanel;
                case UIKey.Tutorial:
                    _tutorialPanel.SetActive(false);
                    _tutorialPanel.transform.SetAsLastSibling();
                    return _tutorialPanel;
                default:
                    return null;
            }
        }

        public void HideAll()
        {
            _menuPanel.SetActive(false);
            _abilitySelectionPanel.SetActive(false);
            _gameHUDPanel.SetActive(false);
            _endGamePanel.SetActive(false);
            _abilityGetPanel.SetActive(false);
            _loadGamePanel.SetActive(false);
            _treasureGetPanel.SetActive(false);
            _mapPanel.SetActive(false);
            _chooseSaveSlotPanel.SetActive(false);
            _tutorialPanel.SetActive(false);
        }

        public bool TryGetUIByKey(UIKey uiKey, out IBaseUI ui)
        {
            switch (uiKey)
            {
                case UIKey.Menu:
                    ui = _menuPanel;
                    return true;
                case UIKey.AbilitySelection:
                    ui = _abilitySelectionPanel;
                    return true;
                case UIKey.GameHUD:
                    ui = _gameHUDPanel;
                    return true;
                case UIKey.EndGame:
                    ui = _endGamePanel;
                    return true;
                case UIKey.AbilityGet:
                    ui = _abilityGetPanel;
                    return true;
                case UIKey.LoadGame:
                    ui = _loadGamePanel;
                    return true;
                case UIKey.TreasureGet:
                    ui = _treasureGetPanel;
                    return true;
                case UIKey.Map:
                    ui = _mapPanel;
                    return true;
                case UIKey.ChooseSaveSlot:
                    ui = _chooseSaveSlotPanel;
                    return true;
                case UIKey.Tutorial:
                    ui = _tutorialPanel;
                    return true;
                default:
                    ui = null;
                    return false;
            }
        }
    }

    public enum UIKey
    {
        Menu,
        AbilitySelection,
        GameHUD,
        EndGame,
        AbilityGet,
        TreasureGet,
        LoadGame,
        Map,
        ChooseSaveSlot,
        Tutorial
    }
}
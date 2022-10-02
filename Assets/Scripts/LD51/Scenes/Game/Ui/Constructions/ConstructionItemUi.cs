using System;
using System.Linq;
using LD51.Data.Tensies;
using LD51.Data.World;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Utils.Extensions;

namespace LD51.Game.Ui.Constructions {
	public class ConstructionItemUi : MonoBehaviour {
		[SerializeField] protected ConstructionSiteModule      _construction;
		[SerializeField] protected Button                      _button;
		[SerializeField] protected Image                       _portrait;
		[SerializeField] protected TMP_Text                    _name;
		[SerializeField] protected WorldObjectResourceAmountUi _resourcesList;

		public static ConstructionSiteModule.Event onAnyClick { get; } = new ConstructionSiteModule.Event();

		private void Start() {
			_button.onClick.AddListenerOnce(() => onAnyClick.Invoke(_construction));
		}

		public void Init() {
			_portrait.sprite = _construction.resultPrefab.GetDefaultWhiteOutlineSprite();
			_name.text = _construction.resultPrefab.name;
			_resourcesList.Refresh(_construction.requiredResources.ToDictionary(t => t.resource, t => t.amount), true);
		}
	}
}
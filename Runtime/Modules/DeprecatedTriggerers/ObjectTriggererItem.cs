using System;
using Devdog.General;
using UnityEngine;

namespace Devdog.InventoryPro
{
    /// <summary>
    ///     Used to trigger item pickup, modify the settings in ObjectTriggererHandler.
    /// </summary>
    [Obsolete("REPLACED BY ItemTrigger")]
//    [ReplacedBy(typeof(ItemTrigger))] // Do this one manually...
    public class ObjectTriggererItem : ObjectTriggererBase
    {
        public override bool triggerMouseClick { get; set; }
        public override KeyCode triggerKeyCode { get; set; }
        public override CursorIcon cursorIcon { get; set; }
        public override Sprite uiIcon { get; set; }
    }
}
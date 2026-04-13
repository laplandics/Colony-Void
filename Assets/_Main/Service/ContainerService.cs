using System.Collections.Generic;
using Cmd;
using Constant;
using ObservableCollections;
using R3;
using Space;
using View.UI.Container;

namespace Service
{
    public class ContainerService
    {
        private readonly CommandProcessor _cmd;
        private readonly UI _root;
        private readonly Dictionary<string, ContainerVm> _containerVmsMap = new();

        public ContainerService(CommandProcessor cmd, UI root, ObservableList<Data.Proxy.UIElement> uiElements)
        {
            _cmd = cmd;
            _root = root;
            
            uiElements.ForEach(CreateContainer);
            uiElements.ObserveAdd().Subscribe(addEvent => CreateContainer(addEvent.Value));
            uiElements.ObserveRemove().Subscribe(removeEvent => DestroyContainer(removeEvent.Value));
        }

        public bool AddContainer(Enums.Containers containerType)
        {
            var command = new CmdCommandAddContainer(containerType);
            var result = _cmd.Process(command);
            return result;
        }

        public bool RemoveContainer(string id)
        {
            var command = new CmdCommandRemoveContainer(id);
            var result = _cmd.Process(command);
            return result;
        }
        
        private void CreateContainer(Data.Proxy.UIElement uiElement)
        {
            if (uiElement is not Data.Proxy.Container container) return;
            var vm = new ContainerVm(container, this);
            _containerVmsMap.Add(vm.ID, vm);
            _root.AddUi(vm);
        }

        private void DestroyContainer(Data.Proxy.UIElement uiElement)
        {
            if (uiElement is not Data.Proxy.Container container) return;
            var vm = _containerVmsMap[container.ID];
            _containerVmsMap.Remove(container.ID);
            _root.RemoveUi(vm);
        }
    }
}
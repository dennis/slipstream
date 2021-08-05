using Slipstream.Components.WinFormUI.Services;
using System;
using System.Windows.Forms;
using System.Linq;
using System.Threading.Tasks;
using Slipstream.Components.WinFormUI.Models;
using Slipstream.Shared;

namespace Slipstream.Components.WinFormUI.Forms
{
    public partial class EventTestWindow : Form
    {
        private EventInfoModel? SelectedEvent = null;

        public IEventBus EventBus { get; }

        public EventTestWindow(IEventBus eventBus)
        {
            EventBus = eventBus;
            InitializeComponent();
        }

        private async void EventTestWindow_Load(object sender, EventArgs e)
        {
            await SetEvents(this.comboEvents);
        }

        private async Task SetEvents(ComboBox comboEvents)
        {
            var bindingSource = new BindingSource
            {
                DataSource = await EventDataService.FindEvents((e) => !e.Properties.Any(p => p.IsComplex))
            };

            comboEvents.DataSource = bindingSource;
            comboEvents.DisplayMember = "Name";
        }

        private void ComboEvents_SelectedIndexChanged(object sender, EventArgs e)
        {
            object? selectedItem = ((ComboBox)sender).SelectedItem;
            SelectedEvent = selectedItem as EventInfoModel;

            if (SelectedEvent != null)
                AddControlsToPanel(SelectedEvent);
        }

        private void AddControlsToPanel(EventInfoModel selectedItem)
        {
            if (selectedItem == null)
            {
                return;
            }

            //var labels = selectedItem.Properties.Select(p => new Label { Text = p.Name });
            //// ToDo: A massive cheat, this could cause a naming collision
            //var textboxes = selectedItem.Properties.Select(p => new TextBox { Name = p.Name });
            //var descriptions = selectedItem.Properties.Select(p => new Label { Text = p.Description });
            //var inputControls = labels.Zip(textboxes, (label, textbox) => CreateInputControl(label, textbox));

            var inputControls = selectedItem.Properties.Select(p => CreateInputControl(p.Name, p.Name, p.Description));

            flpControls.Controls.Clear();
            flpControls.Controls.AddRange(inputControls.ToArray());
        }

        private Panel CreateInputControl(string label, string textbox, string Description)
        {
            var panelControl = new FlowLayoutPanel();
            var labelControl = new Label { Text = label };
            var textboxControl = new TextBox { Name = textbox, Width = 150 };
            var descriptionControl = new Label { Text = Description, Width = 150 };

            panelControl.Controls.Add(labelControl);
            panelControl.Controls.Add(textboxControl);
            panelControl.Controls.Add(descriptionControl);

            return panelControl;
        }

        private void BtnCreateEvent_Click(object sender, EventArgs e)
        {
            if (SelectedEvent != null)
            {
                var eventInstance = CreateEventFromForm(SelectedEvent, flpControls);

                if (eventInstance != null)
                {
                    EventBus.PublishEvent(eventInstance);
                }
            }
        }

        private IEvent? CreateEventFromForm(EventInfoModel eventInfo, FlowLayoutPanel flpControls)
        {
            // ToDo: A massive cheat, this could cause a naming collision
            // using it to bind data

            object? obj = Activator.CreateInstance(eventInfo.EventType);
            if (obj == null)
            {
                return null;
            }

            foreach (var prop in eventInfo.Properties)
            {
                var inputControl = flpControls.Controls.Find(prop.Name, true).First() as TextBox;

                if (inputControl != null)
                {
                    obj.GetType().GetProperty(prop.Name)?.SetValue(obj, Convert.ChangeType(inputControl.Text, prop.Type));
                }
            }

            return (IEvent?)obj;
        }
    }
}
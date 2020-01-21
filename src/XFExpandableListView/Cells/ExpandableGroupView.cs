using Xamarin.Forms;
using XFExpandableListView.Abstractions;

namespace XFExpandableListView.Cells
{
    public class ExpandableGroupView : ContentView
    {
        #region [Bindable Properties]

        /// <summary>
        /// The Bindable Property for the GroupHeader Command
        /// </summary>
        public static BindableProperty CellCommandProperty = BindableProperty.Create(
            nameof(CellCommand), typeof(Command), typeof(ExpandableGroupCell));

        /// <summary>
        /// The Command that will be executed on the cell click
        /// </summary>
        public Command CellCommand
        {
            get => (Command)GetValue(CellCommandProperty);
            set => SetValue(CellCommandProperty, value);
        }

        /// <summary>
        /// The Parameter that will be passed into the Cell Command
        /// </summary>
        public static BindableProperty CellCommandParameterProperty = BindableProperty.Create(
            nameof(CellCommandParameter), typeof(object), typeof(ExpandableGroupCell));

        /// <summary>
        /// The Command that will be executed on the group header click
        /// </summary>
        public object CellCommandParameter
        {
            get => GetValue(CellCommandParameterProperty);
            set => SetValue(CellCommandParameterProperty, value);
        }

        #endregion

        private IExpandableGroup GroupController => BindingContext as IExpandableGroup;
        private IExpandableController ExpandableController => Parent as IExpandableController;

        public ExpandableGroupView()
        {
            GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(() =>
                {
                    #region [Toggle Group]

                    /* If is collapsing enabled, then collapse/expand the group */
                    if (ExpandableController.IsCollapsingEnabled)
                    {
                        ExpandableController.ToggleGroup(GroupController);
                    }

                    #endregion

                    #region [Execute Cell Command]

                    /* Pass the Cell Command Parameter if it is not null, otherwise pass the group */
                    var cellParameter = CellCommandParameter ?? GroupController;
                    if (CellCommand != null && CellCommand.CanExecute(cellParameter))
                    {
                        CellCommand.Execute(cellParameter);
                    }

                    #endregion

                    #region [Execute ListView Command]

                    /* Pass the GroupHeader Command Parameter if it is not null, otherwise pass the group */
                    var parameter = ExpandableController.GroupHeaderCommandParameter ?? GroupController;
                    if (ExpandableController.GroupHeaderCommand != null && ExpandableController.GroupHeaderCommand.CanExecute(cellParameter))
                    {
                        ExpandableController.GroupHeaderCommand.Execute(parameter);
                    }

                    #endregion

                    #region [Invoke Event]

                    /* Invoke the Group Clicked Event */
                    ExpandableController.OnGroupClicked(GroupController);

                    #endregion
                })
            });
        }
    }
}

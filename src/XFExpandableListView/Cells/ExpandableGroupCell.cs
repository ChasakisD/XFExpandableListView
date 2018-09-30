using Xamarin.Forms;
using XFExpandableListView.Abstractions;

namespace XFExpandableListView.Cells
{
    /// <inheritdoc />
    /// <summary>
    /// This Cell manages the Expanding/Collapsing of the group.
    /// Use it as a ViewCell
    /// </summary>
    public class ExpandableGroupCell : ViewCell
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

        private bool _holderAdded;

        private IExpandableGroup GroupController => BindingContext as IExpandableGroup;
        private IExpandableController ExpandableController => Parent as IExpandableController;

        protected override void OnChildAdded(Element child)
        {
            base.OnChildAdded(child);

            /* Lock for infinite loop because of View = holder */
            if (_holderAdded)
            {
                _holderAdded = false;
                return;
            }

            if (!(child is View view)) return;

            _holderAdded = true;

            View.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(async () =>
                {
                    #region [Toggle Group]

                    /* If is collapsing enabled, then collapse/expand the group */
                    if (ExpandableController.IsCollapsingEnabled)
                    {
                        await ExpandableController.ToggleGroup(GroupController);
                    }

                    #endregion

                    #region [Execute Cell Command]

                    /* Execute the cell command */
                    if (CellCommand == null) return;

                    /* Pass the Cell Command Parameter if it is not null, otherwise pass the group */
                    var cellParameter = CellCommandParameter ?? GroupController;
                    if (CellCommand != null && CellCommand.CanExecute(cellParameter))
                    {
                        CellCommand.Execute(cellParameter);
                    }


                    #endregion

                    #region [Execute ListView Command]

                    /* Execute the group header command */
                    if (ExpandableController.GroupHeaderCommand == null) return;


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

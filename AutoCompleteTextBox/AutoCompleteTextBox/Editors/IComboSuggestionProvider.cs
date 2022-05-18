using System.Collections;
using System.Threading.Tasks;


namespace AutoCompleteTextBox.Editors
{
    public interface IComboSuggestionProvider
    {

        #region Public Methods

        Task<IEnumerable> GetSuggestions(string filter);
        Task<IEnumerable> GetFullCollection();

        #endregion Public Methods
    }
}

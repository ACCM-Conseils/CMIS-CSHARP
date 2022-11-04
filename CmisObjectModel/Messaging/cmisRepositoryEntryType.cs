
namespace CmisObjectModel.Messaging
{
    public partial class cmisRepositoryEntryType
    {

        protected Core.cmisRepositoryInfoType _repository;
        public Core.cmisRepositoryInfoType Repository
        {
            get
            {
                if (_repository is null)
                    _repository = new Core.cmisRepositoryInfoType() { RepositoryId = _repositoryId, RepositoryName = _repositoryName };
                return _repository;
            }
            set
            {
                if (!ReferenceEquals(_repository, value))
                {
                    var oldValue = _repository;
                    _repository = value;
                    OnPropertyChanged("Repository", value, oldValue);
                    if (value is null)
                    {
                        RepositoryId = null;
                        RepositoryName = null;
                    }
                    else
                    {
                        RepositoryId = value.RepositoryId;
                        RepositoryName = value.RepositoryName;
                    }
                }
            }
        }

        public static implicit operator cmisRepositoryEntryType(Core.cmisRepositoryInfoType value)
        {
            return value is null ? null : new cmisRepositoryEntryType() { _repository = value, _repositoryId = value.RepositoryId, _repositoryName = value.RepositoryName };
        }

    }
}
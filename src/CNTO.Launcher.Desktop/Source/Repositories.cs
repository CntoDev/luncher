using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CNTO.Launcher;
using CNTO.Launcher.Infrastructure;

namespace UI.Source
{
    public class Repositories : INotifyPropertyChanged
    {
        public Repositories()
        {
            HeadlessClientNumber = 1;
        }

        private List<RepositoryDto> _repositories;

        public IEnumerable<RepositoryDto> All => _repositories;        

        public IEnumerable<RepositoryDto> GetSelected() => _repositories.Where(r => r.Selected);

        public int HeadlessClientNumber { get; set; }

        public bool GM { get; set; }

        public bool VN { get; set; }

        public bool CSLA { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public void Load(IRepositoryCollection collection)
        {
            _repositories = collection.All().Select(c => new RepositoryDto()
            {
                Identity = c.RepositoryId.Name,
                Path = c.Path,
                Selected = false,
                ServerSide = c.ServerSide ? "Yes" : string.Empty
            }).ToList();

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("All"));
        }
    }
}

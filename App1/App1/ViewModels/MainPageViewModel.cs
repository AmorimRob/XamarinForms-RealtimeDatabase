using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Firebase.Database;
using Firebase.Database.Query;
using Firebase.Database.Streaming;
using Xamarin.Forms;
using App1.ViewModels;
using App1.Models;

namespace App1
{
    public class MainPageViewModel : BaseViewModel
    {
        private readonly string ENDERECO_FIREBASE = "https://demoapp-1df56.firebaseio.com/";
        private readonly FirebaseClient _firebaseClient;

        private ObservableCollection<Pedido> _pedidos;

        public ObservableCollection<Pedido> Pedidos
        {
            get { return _pedidos; }
            set { _pedidos = value; OnPropertyChanged();}
        }

        public Pedido PedidoSelecionado;

        public ICommand AceitarPedidoCmd { get; set; }

        public MainPageViewModel()
        {
            _firebaseClient = new FirebaseClient(ENDERECO_FIREBASE);
            Pedidos = new ObservableCollection<Pedido>();
            AceitarPedidoCmd = new Command(() => AceitarPedido());
            ListenerPedidos();
        }

        private void AceitarPedido()
        {
            PedidoSelecionado.IdVendedor = 1;
            _firebaseClient
                .Child("pedidos")
                .Child(PedidoSelecionado.KeyPedido)
                .PutAsync(PedidoSelecionado);
        }

        private void ListenerPedidos()
        {
            _firebaseClient
                .Child("pedidos")
                .AsObservable<Pedido>()
                .Subscribe(d =>
                {
                    if (d.EventType == FirebaseEventType.InsertOrUpdate)
                    {
                        if (d.Object.IdVendedor == 0)
                            AdicionarPedido(d.Key, d.Object);
                        else
                            RemoverPedido(d.Key);
                    }
                    else if (d.EventType == FirebaseEventType.Delete)
                    {
                        RemoverPedido(d.Key);
                    }
                });
        }

        private void AdicionarPedido(string key, Pedido pedido)
        {
            Pedidos.Add(new Pedido()
            {
                KeyPedido = key,
                Cliente = pedido.Cliente,
                Produto = pedido.Produto,
                Preco = pedido.Preco
            });
        }

        private void RemoverPedido(string pedidoKey)
        {
            var pedido = Pedidos.FirstOrDefault(x => x.KeyPedido == pedidoKey);
            Pedidos.Remove(pedido);
        }
    }
}

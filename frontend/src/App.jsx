import './App.css'
import { BrowserRouter as Router, Routes, Route, Link } from "react-router-dom";
import NewOrder from './pages/NewOrder';
import Orders from './pages/Orders';
import OrderDetails from './pages/OrderDetails';

function App() {
  return (
    <Router>
      <nav className="p-5 flex flex-row gap-3 bg-gray-200">
        <Link className="bg-gray-400 font-semibold p-1 rounded-md"
          to="/novo-pedido"
        >Novo pedido</Link>
        <Link className="bg-gray-400 font-semibold p-1 rounded-md"
          to="/listagem-pedidos"
        >Pedidos</Link>
      </nav>

      <Routes>
        <Route path="/novo-pedido" element={<NewOrder />} />
        <Route path="/listagem-pedidos" element={<Orders />} />
        <Route path="/detalhes-pedidos" element={<OrderDetails />} />
      </Routes>
    </Router>
  );
}

export default App

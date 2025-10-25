import { useState } from "react";
import { NewOrder } from "../services/OrdersService";
import { useNavigate } from 'react-router-dom';

export default function OrderForm() {
    const navigate = useNavigate();

    const [cliente, setCliente] = useState("");
    const [produto, setProduto] = useState("");
    const [valor, setValor] = useState("");

    async function handleSubmit(e) {
        e.preventDefault();

        if (!cliente || !produto || !valor) {
            alert("Preencha todos os campos!");
            return;
        }

        try {
            await NewOrder(
                cliente,
                produto,
                parseFloat(valor)
            );

            navigate(`/detalhes-pedido/${id}`);
        } catch (err) {
            console.error(err);
            alert("Erro ao enviar o pedido!");
        }
    }

    return (
        <form onSubmit={handleSubmit}>
            <div className="max-w-md mx-auto border border-slate-200 px-5 py-2 bg-slate-100">
                <div className="my-3 max-w flex flex-col">
                    <label htmlFor="clienteInput" className="font-semibold">Cliente</label>
                    <input className="bg-white border-b border-slate-300 focus:border-slate-900 block min-w-0 grow py-1.5 pr-3 pl-1 text-base text-slate-900 placeholder:text-slate-400 focus:outline-none sm:text-sm/6"
                        id="clienteInput"
                        type="text"
                        onChange={(e) => setCliente(e.target.value)}
                    ></input>
                </div>
                <div className="my-3 max-w flex flex-col">
                    <label htmlFor="produtoInput" className="font-semibold">Produto</label>
                    <input className="bg-white border-b border-slate-300 focus:border-slate-900 block min-w-0 grow py-1.5 pr-3 pl-1 text-base text-slate-900 placeholder:text-slate-400 focus:outline-none sm:text-sm/6"
                        id="produtoInput"
                        type="text"
                        onChange={(e) => setProduto(e.target.value)}
                    ></input>
                </div>
                <div className="my-3 max-w flex flex-col">
                    <label htmlFor="valorInput" className="font-semibold">Valor</label>
                    <div className="bg-white flex flex-row items-center">
                        <span>R$</span>
                        <input className="bg-white border-b border-slate-300 focus:border-slate-900 block min-w-0 grow py-1.5 pr-3 pl-1 text-base text-slate-900 placeholder:text-slate-400 focus:outline-none sm:text-sm/6"
                            id="valorInput"
                            type="number"
                            onChange={(e) => setValor(e.target.value)}
                        ></input>
                    </div>
                </div>
                <div className="my-2 max-w text-center">
                    <button type="submit" className="p-2 border border-slate-200 rounded-xl bg-blue-200 cursor-pointer">Fazer pedido</button>
                </div>
            </div>
        </form>
    );
}
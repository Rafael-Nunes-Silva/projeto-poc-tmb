import { useState } from "react";
import { NewOrder } from "../services/OrdersService";

export default function OrderForm() {
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
            // Navegar para outra página
        } catch (err) {
            console.error(err);
            alert("Erro ao enviar o pedido!");
        }
    }

    return (
        <form onSubmit={handleSubmit}>
            <div className="max-w-md mx-auto border border-gray-200 px-5 py-2 bg-gray-100">
                <div className="my-3 max-w flex flex-col">
                    <label for="clienteInput" className="font-semibold">Cliente</label>
                    <input className="bg-white border-b border-gray-300 focus:border-gray-900 block min-w-0 grow py-1.5 pr-3 pl-1 text-base text-gray-900 placeholder:text-gray-400 focus:outline-none sm:text-sm/6"
                        id="clienteInput"
                        type="text"
                        onChange={(e) => setCliente(e.target.value)}
                    ></input>
                </div>
                <div className="my-3 max-w flex flex-col">
                    <label for="produtoInput" className="font-semibold">Produto</label>
                    <input className="bg-white border-b border-gray-300 focus:border-gray-900 block min-w-0 grow py-1.5 pr-3 pl-1 text-base text-gray-900 placeholder:text-gray-400 focus:outline-none sm:text-sm/6"
                        id="produtoInput"
                        type="text"
                        onChange={(e) => setProduto(e.target.value)}
                    ></input>
                </div>
                <div className="my-3 max-w flex flex-col">
                    <label for="valorInput" className="font-semibold">Valor</label>
                    <div className="bg-white flex flex-row items-center">
                        <span>R$</span>
                        <input className="bg-white border-b border-gray-300 focus:border-gray-900 block min-w-0 grow py-1.5 pr-3 pl-1 text-base text-gray-900 placeholder:text-gray-400 focus:outline-none sm:text-sm/6"
                            id="valorInput"
                            type="number"
                            onChange={(e) => setValor(e.target.value)}
                        ></input>
                    </div>
                </div>
                <div className="my-2 max-w text-center">
                    <button type="submit" className="p-2 rounded-xl bg-blue-200 cursor-pointer">Fazer pedido</button>
                </div>
            </div>
        </form>
    );
}
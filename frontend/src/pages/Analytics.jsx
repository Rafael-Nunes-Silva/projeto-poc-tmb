import { useState } from "react";
import { Analise } from "../services/OrdersService";
import Loading from "../components/Loading";

export default function Analytics() {
    const [carregando, setCarregando] = useState(false);

    const [pergunta, setPegunta] = useState();
    const [resposta, setResposta] = useState("");

    async function handleSubmit(e) {
        e.preventDefault();

        console.log("A");

        if (!pergunta) {
            return;
        }

        try {
            setCarregando(true);
            const response = await Analise(pergunta);
            console.log(response);

            setResposta(response.data);
            setCarregando(false);
        } catch (err) {
            console.error(err);
            alert("Erro ao enviar pergunta!");
        }
    }

    return (
        <div className="mx-auto mt-75">
            <form onSubmit={handleSubmit}>
                <div className="max-w-md mx-auto border border-slate-200 px-5 py-2 bg-slate-100">
                    {!carregando ? <Loading></Loading> : <></>}
                    <div className="my-3 max-w flex flex-col bg-white">
                        <p>{resposta}</p>
                    </div>
                    <div className="my-3 max-w flex flex-col">
                        <input className="bg-white border-b border-slate-300 focus:border-slate-900 block min-w-0 grow py-1.5 pr-3 pl-1 text-base text-slate-900 placeholder:text-slate-400 focus:outline-none sm:text-sm/6"
                            id="perguntaInput"
                            type="text"
                            placeholder="Faça sua pergunta"
                            onChange={(e) => setPegunta(e.target.value)}
                        ></input>
                    </div>
                    <div className="my-2 max-w text-center">
                        <button
                            type="submit"
                            className="p-2 border border-slate-200 rounded-xl bg-blue-200 cursor-pointer"
                            disabled={carregando}
                        >Enviar</button>
                    </div>
                </div>
            </form>
        </div>
    );
}
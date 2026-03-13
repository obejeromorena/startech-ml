    import { useNavigate } from "react-router-dom";

    export default function Dashboard() {

    const navigate = useNavigate();

    return (
        <div>
        <h1>Startech ML</h1>

        <button onClick={() => navigate("/publications")}>
            Ver publicaciones
        </button>

        <button onClick={() => navigate("/create")}>
            Crear publicación
        </button>
        </div>
    );
    }
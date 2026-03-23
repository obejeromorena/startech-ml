    import { useNavigate } from "react-router-dom";

    export default function Dashboard() {
    const navigate = useNavigate();

    const styles = {
        container: {
        minHeight: "100vh",
        width: "100vw",
        background: "linear-gradient(135deg, #0a0a0a, #0f172a, #020617)",
        color: "white",
        display: "flex",
        flexDirection: "column",
        alignItems: "stretch",
        justifyContent: "flex-start",
        fontFamily: "Arial, sans-serif",
        },
        header: {
        padding: "30px",
        fontSize: "40px",
        color: "#3b82f6",
        },
        content: {
        flex: 1,
        display: "flex",
        gap: "20px",
        padding: "30px",
        },
        card: {
        flex: 1,
        backgroundColor: "#111827",
        padding: "40px",
        borderRadius: "0px",
        boxShadow: "0 0 20px rgba(59,130,246,0.2)",
        display: "flex",
        flexDirection: "column",
        gap: "20px",
        justifyContent: "center",
        alignItems: "center",
        },
        buttonPrimary: {
        backgroundColor: "#2563eb",
        border: "none",
        padding: "15px 25px",
        borderRadius: "10px",
        color: "white",
        cursor: "pointer",
        fontWeight: "bold",
        fontSize: "16px",
        },
        buttonSecondary: {
        backgroundColor: "transparent",
        border: "1px solid #3b82f6",
        padding: "15px 25px",
        borderRadius: "10px",
        color: "white",
        cursor: "pointer",
        fontWeight: "bold",
        fontSize: "16px",
        },
        footer: {
        textAlign: "center",
        padding: "15px",
        fontSize: "12px",
        color: "gray",
        borderTop: "1px solid #1f2937",
        },
    };

    return (
        <div style={styles.container}>
        <div style={styles.header}>Startechnology</div>

        <div style={styles.content}>
            <div style={styles.card}>
            <button
                style={styles.buttonPrimary}
                onClick={() => navigate("/publications")}
            >
                Ver publicaciones
            </button>

            <button
                style={styles.buttonSecondary}
                onClick={() => navigate("/create")}
            >
                Crear publicación
            </button>
            </div>
        </div>

        <div style={styles.footer}>
            © 2026 Startechnology. Innovación sin límites.
        </div>
        </div>
    );
    }

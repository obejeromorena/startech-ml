export default function PublicationTable({ publications, onDelete, onEdit }) {

    const styles = {
        container: {
            width: "100vw",
            minHeight: "100vh",
            background: "linear-gradient(135deg, #0a0a0a, #0f172a, #020617)",
            color: "white",
            padding: "30px",
            fontFamily: "Arial, sans-serif",
            boxSizing: "border-box",
        },
        title: {
            fontSize: "32px",
            marginBottom: "20px",
            color: "#3b82f6",
        },
        table: {
            width: "100%",
            borderCollapse: "collapse",
            backgroundColor: "#111827",
            boxShadow: "0 0 20px rgba(59,130,246,0.2)",
        },
        th: {
            padding: "15px",
            backgroundColor: "#1f2937",
            borderBottom: "1px solid #374151",
            textAlign: "left",
        },
        td: {
            padding: "12px",
            borderBottom: "1px solid #374151",
        },
        row: {
            transition: "0.2s",
        },
        actions: {
            display: "flex",
            gap: "10px",
        },
        btnEdit: {
            backgroundColor: "#2563eb",
            border: "none",
            padding: "8px 12px",
            borderRadius: "6px",
            color: "white",
            cursor: "pointer",
        },
        btnDelete: {
            backgroundColor: "#dc2626",
            border: "none",
            padding: "8px 12px",
            borderRadius: "6px",
            color: "white",
            cursor: "pointer",
        },
    };

    const dataToShow =
        publications && publications.length > 0
            ? publications
            : [
                { id: "1", title: "Mouse Gamer", price: 15000, status: "active", category_id: "MLA3530" },
                { id: "2", title: "Teclado Mecánico", price: 30000, status: "paused", category_id: "MLA3530" }
            ];

    return (
        <div style={styles.container}>
            <h1 style={styles.title}>Publicaciones</h1>

            <table style={styles.table}>
                <thead>
                    <tr>
                        <th style={styles.th}>ID</th>
                        <th style={styles.th}>Titulo</th>
                        <th style={styles.th}>Precio</th>
                        <th style={styles.th}>Estado</th>
                        <th style={styles.th}>Categoria</th>
                        <th style={styles.th}>Acciones</th>
                    </tr>
                </thead>

                <tbody>
                    {dataToShow.map((p) => (
                        <tr key={p.id} style={styles.row}>
                            <td style={styles.td}>{p.id}</td>
                            <td style={styles.td}>{p.title}</td>
                            <td style={styles.td}>${p.price}</td>
                            <td style={styles.td}>{p.status}</td>
                            <td style={styles.td}>{p.category_id}</td>

                            <td style={styles.td}>
                                <div style={styles.actions}>
                                    
                                    <button
                                        style={styles.btnEdit}
                                        onClick={() => onEdit(p)}
                                    >
                                        Editar
                                    </button>

                                    <button
                                        style={styles.btnDelete}
                                        onClick={() => onDelete(p.id)}
                                    >
                                        Eliminar
                                    </button>
                                </div>
                            </td>
                        </tr>
                    ))}
                </tbody>
            </table>
        </div>
    );
}
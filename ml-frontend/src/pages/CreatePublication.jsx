    import { useState } from "react";
    import { createPublication } from "../services/Api";

    export default function CreatePublication() {
    const [title, setTitle] = useState("");
    const [price, setPrice] = useState("");
    const [category, setCategory] = useState("");
    const [image, setImage] = useState("");

    async function handleSubmit(e) {
        e.preventDefault();

        if (!title || !price || !category || !image) {
        alert("Completa todos los campos");
        return;
        }

        if (isNaN(price)) {
        alert("El precio debe ser un número");
        return;
        }

        const data = {
        title: title,
        price: parseFloat(price),
        category_id: category,
        available_quantity: 1,
        buying_mode: "buy_it_now",
        listing_type_id: "gold_special",
        condition: "new",
        currency_id: "ARS",
        pictures: [{ source: image }],
        attributes: [
            { id: "BRAND", value_name: "Logitech" },
            { id: "MODEL", value_name: "G203" },
        ],
        };

        try {
        await createPublication(data);

        alert("Publicación creada correctamente");

        setTitle("");
        setPrice("");
        setCategory("");
        setImage("");
        } catch (error) {
        console.error(error);
        alert("Error al crear publicación");
        }
    }

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
        title: {
        fontSize: "40px",
        margin: "30px",
        color: "#3b82f6",
        },
        card: {
        backgroundColor: "#111827",
        padding: "40px",
        borderRadius: "0px",
        boxShadow: "0 0 25px rgba(59,130,246,0.25)",
        display: "grid",
        gridTemplateColumns: "1fr 1fr",
        gap: "20px",
        width: "100%",
        height: "100%",
        },
        inputGroup: {
        display: "flex",
        flexDirection: "column",
        gap: "5px",
        },
        input: {
        padding: "12px",
        borderRadius: "8px",
        border: "none",
        outline: "none",
        backgroundColor: "#1f2937",
        color: "white",
        },
        button: {
        marginTop: "10px",
        backgroundColor: "#2563eb",
        border: "none",
        padding: "14px",
        borderRadius: "10px",
        color: "white",
        cursor: "pointer",
        fontWeight: "bold",
        fontSize: "16px",
        gridColumn: "1 / -1",
        },
    };

    return (
        <div style={styles.container}>
        <h1 style={styles.title}>Crear publicación</h1>

        <form onSubmit={handleSubmit} style={styles.card}>
            <div style={styles.inputGroup}>
            <label>Título</label>
            <input
                style={styles.input}
                value={title}
                onChange={(e) => setTitle(e.target.value)}
            />
            </div>

            <div style={styles.inputGroup}>
            <label>Precio</label>
            <input
                style={styles.input}
                type="number"
                value={price}
                onChange={(e) => setPrice(e.target.value)}
            />
            </div>

            <div style={styles.inputGroup}>
            <label>Categoría</label>
            <input
                style={styles.input}
                value={category}
                onChange={(e) => setCategory(e.target.value)}
                placeholder="MLA3530"
            />
            </div>

            <div style={styles.inputGroup}>
            <label>Imagen URL</label>
            <input
                style={styles.input}
                value={image}
                onChange={(e) => setImage(e.target.value)}
            />
            </div>

            <button type="submit" style={styles.button}>
            Crear publicación
            </button>
        </form>
        </div>
    );
    }

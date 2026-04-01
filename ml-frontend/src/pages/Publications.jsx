import { useEffect, useState } from "react" 
import { getPublications, deletePublication } from "../services/Api"
import PublicationTable from "../components/PublicationTable" 

export default function Publications() {

    const [publications, setPublications] = useState([])

    const [editingPub, setEditingPub] = useState(null)
    const [newPrice, setNewPrice] = useState("")
    const [newTitle, setNewTitle] = useState("")

    async function loadPublications() {
        try {
            const data = await getPublications()
            setPublications(data)
        } catch (error) {
            console.error(error)
            alert("Error al cargar publicaciones")
        }
    }

    useEffect(() => {
        loadPublications()
    }, [])

    async function handleDelete(id) {

        if (!confirm("¿Seguro que querés cerrar la publicación?"))
            return

        try {

            await deletePublication(id)

            alert("Publicación cerrada correctamente")

            await loadPublications()

        } catch (error) {

            console.error(error)

            alert("Error al cerrar la publicación")

        }
    }

    function handleEdit(pub) {
        console.log("EDITANDO:", pub) 

        setEditingPub(pub)
        setNewPrice(pub.price)
        setNewTitle(pub.title)
    }

    async function handleUpdate() {

        if (!newPrice || isNaN(newPrice)) {
            alert("Precio inválido")
            return
        }

        if (!newTitle) {
            alert("El título no puede estar vacío")
            return
        }

            try {
            const res = await fetch(`https://localhost:7006/api/publications/${editingPub.id}`, {
                method: "PUT",
                headers: {
                    "Content-Type": "application/json"
                },
                body: JSON.stringify({
                    price: parseFloat(newPrice)
                })
            })

            const data = await res.json()

            console.log("RESPUESTA UPDATE:", data) // 👈 ACÁ VA

            if (!res.ok) {
                throw new Error("Error al actualizar")
            }

            alert("Actualizado correctamente")

            setEditingPub(null)
            loadPublications()

        } catch (error) {
            console.error(error)
            alert("Error al actualizar la publicación")
        }
    }

    return (
        <div>

            <PublicationTable
                publications={publications}
                onDelete={handleDelete}
                onEdit={handleEdit} // 👈 importante: ahora recibe el objeto completo
            />

            {/* 🆕 FORMULARIO */}
            {editingPub && (
                <div style={{ marginTop: "20px", border: "1px solid black", padding: "10px" }}>
                    <h2>Editar publicación</h2>

                    <div>
                        <label>Título</label>
                        <input
                            value={newTitle}
                            onChange={(e) => setNewTitle(e.target.value)}
                        />
                    </div>

                    <div>
                        <label>Precio</label>
                        <input
                            type="number"
                            value={newPrice}
                            onChange={(e) => setNewPrice(e.target.value)}
                        />
                    </div>

                    <button onClick={handleUpdate}>
                        Guardar cambios
                    </button>

                    <button onClick={() => setEditingPub(null)}>
                        Cancelar
                    </button>
                </div>
            )}

        </div>
    )
}
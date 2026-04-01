import { useEffect, useState } from "react" 
import { getPublications, deletePublication } from "../services/Api"
import PublicationTable from "../components/PublicationTable" 

export default function Publications() {

    const [publications, setPublications] = useState([])

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

    async function handleEdit(id) {

        const newPrice = prompt("Nuevo precio:")

        if (!newPrice || isNaN(newPrice)) {
            alert("Precio inválido")
            return
        }

        try {

            const res = await fetch(`https://localhost:7006/api/publications/${id}`, {
                method: "PUT",
                headers: {
                    "Content-Type": "application/json"
                },
                body: JSON.stringify({
                    price: parseFloat(newPrice)
                })
            })

            if (!res.ok) {
                throw new Error("Error al actualizar precio")
            }

            alert("Precio actualizado correctamente")

            await loadPublications()

        } catch (error) {

            console.error(error)

            alert("Error al actualizar la publicación")

        }
    }

    // ❌ ELIMINAMOS handleSubmit de abajo (no se usaba y estaba mal ubicado)

    // ✅ USAMOS EL COMPONENTE NUEVO
    return (
        <PublicationTable
            publications={publications}
            onDelete={handleDelete}
            onEdit={handleEdit}
        />
    )
}
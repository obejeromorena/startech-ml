import { useEffect, useState } from "react"
import { getPublications, deletePublication } from "../services/Api"

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

        // validar precio
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

    return (
        <div>

            <h1>Publicaciones</h1>

            <table border="1">

                <thead>
                    <tr>
                        <th>ID</th>
                        <th>Titulo</th>
                        <th>Precio</th>
                        <th>Estado</th>
                        <th>Acciones</th>
                    </tr>
                </thead>

                <tbody>

                    {publications.map(pub => (

                        <tr key={pub.id}>

                            <td>{pub.id}</td>
                            <td>{pub.title}</td>
                            <td>{pub.price}</td>
                            <td>{pub.status}</td>

                            <td>

                                <button onClick={() => handleEdit(pub.id)}>
                                    Editar
                                </button>

                                <button onClick={() => handleDelete(pub.id)}>
                                    Eliminar
                                </button>

                                <a href={pub.permalink} target="_blank" rel="noreferrer">
                                    Ver en ML
                                </a>

                            </td>

                        </tr>

                    ))}

                </tbody>

            </table>

        </div>
    )

    async function handleSubmit(e) {
    e.preventDefault()

    try {

        const res = await fetch("https://localhost:7006/api/publications", {
            method: "POST",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify(formData)
        })

        if (!res.ok) {
            const text = await res.text()
            throw new Error(text)
        }

        const data = await res.json()

        alert("Publicación creada correctamente")

    } catch (error) {

        console.error(error)

        alert(error.message)

    }
    }
}
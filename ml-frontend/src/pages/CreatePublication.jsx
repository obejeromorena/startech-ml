import { useState } from "react"
import { createPublication } from "../services/Api"

export default function CreatePublication() {

    const [title, setTitle] = useState("")
    const [price, setPrice] = useState("")
    const [category, setCategory] = useState("")
    const [image, setImage] = useState("")

    async function handleSubmit(e) {

    e.preventDefault()

    const data = {

        title: title,
        price: parseFloat(price),
        category_id: category,
        available_quantity: 1,
        buying_mode: "buy_it_now",
        listing_type_id: "gold_special",
        condition: "new",
        currency_id: "ARS",

        pictures: [
        { source: image }
        ]

    }

    try {

        await createPublication(data)

        alert("Publicación creada correctamente")

        setTitle("")
        setPrice("")
        setCategory("")
        setImage("")

    } catch (error) {

        console.error(error)
        alert("Error al crear publicación")

    }

    }

    return (

    <div>

        <h1>Crear publicación</h1>

        <form onSubmit={handleSubmit}>

        <div>
            <label>Titulo</label>
            <input
            value={title}
            onChange={(e) => setTitle(e.target.value)}
            />
        </div>

        <div>
            <label>Precio</label>
            <input
            type="number"
            value={price}
            onChange={(e) => setPrice(e.target.value)}
            />
        </div>

        <div>
            <label>Categoria</label>
            <input
            value={category}
            onChange={(e) => setCategory(e.target.value)}
            placeholder="MLA3530"
            />
        </div>

        <div>
            <label>Imagen URL</label>
            <input
            value={image}
            onChange={(e) => setImage(e.target.value)}
            />
        </div>

        <button type="submit">
            Crear publicación
        </button>

        </form>

    </div>
    )
}
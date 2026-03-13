    export default function PublicationTable({ publications, onDelete, onEdit }) {

    return (
        <table border="1">
        <thead>
            <tr>
            <th>ID</th>
            <th>Titulo</th>
            <th>Precio</th>
            <th>Estado</th>
            <th>Categoria</th>
            <th>Acciones</th>
            </tr>
        </thead>

        <tbody>
            {publications.map((p) => (
            <tr key={p.id}>
                <td>{p.id}</td>
                <td>{p.title}</td>
                <td>{p.price}</td>
                <td>{p.status}</td>
                <td>{p.category_id}</td>

                <td>
                <button onClick={() => onEdit(p)}>Editar</button>
                <button onClick={() => onDelete(p.id)}>Eliminar</button>
                </td>

            </tr>
            ))}
        </tbody>
        </table>
    );
    }
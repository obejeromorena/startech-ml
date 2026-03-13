    import { useEffect, useState } from "react";
    import { getPublications, deletePublication } from "../services/Api";
    import PublicationTable from "../components/PublicationTable";

    export default function Publications() {

    const [publications, setPublications] = useState([]);

    const userId = "2804901742283043";

    useEffect(() => {
        loadPublications();
    }, []);


    const loadPublications = async () => {
    const data = await getPublications();

    setPublications(data);
};

    const handleDelete = async (id) => {

        await deletePublication(id);

        loadPublications();
    };

    const handleEdit = (publication) => {
        console.log("Editar", publication);
    };

    return (
        <div>

        <h2>Publicaciones</h2>

        <PublicationTable
            publications={publications}
            onDelete={handleDelete}
            onEdit={handleEdit}
        />

        </div>
    );
    }
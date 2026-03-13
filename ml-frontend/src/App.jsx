    import { BrowserRouter, Routes, Route } from "react-router-dom";
    import Dashboard from "./pages/Dashboard";
    import Publications from "./pages/Publications";
    import CreatePublication from "./pages/CreatePublication";

    function App() {

    return (
        <BrowserRouter>

        <Routes>

            <Route path="/" element={<Dashboard />} />

            <Route path="/publications" element={<Publications />} />

            <Route path="/create" element={<CreatePublication />} />

        </Routes>

        </BrowserRouter>
    );
    }

export default App;
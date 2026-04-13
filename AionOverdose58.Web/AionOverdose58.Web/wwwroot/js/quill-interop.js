window.quillInterop = {
    instances: {},

    initialize: function (elementId, initialContent) {
        const el = document.getElementById(elementId);
        if (!el) return;

        const quill = new Quill('#' + elementId, {
            theme: 'snow',
            modules: {
                toolbar: [
                    [{ header: [1, 2, 3, 4, false] }],
                    ['bold', 'italic', 'underline', 'strike'],
                    [{ color: [] }, { background: [] }],
                    [{ align: [] }],
                    [{ list: 'ordered' }, { list: 'bullet' }],
                    ['link', 'image', 'blockquote', 'code-block'],
                    ['clean']
                ]
            }
        });

        if (initialContent) {
            quill.root.innerHTML = initialContent;
        }

        this.instances[elementId] = quill;
    },

    getContent: function (elementId) {
        const quill = this.instances[elementId];
        return quill ? quill.root.innerHTML : '';
    },

    setContent: function (elementId, content) {
        const quill = this.instances[elementId];
        if (quill) quill.root.innerHTML = content || '';
    },

    dispose: function (elementId) {
        delete this.instances[elementId];
    }
};

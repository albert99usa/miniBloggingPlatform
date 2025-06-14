
document.addEventListener('DOMContentLoaded', function () {
    const searchInput = document.getElementById('categorySearch');
    const dropdown = document.getElementById('categoriesDropdown');
    const selectedDisplay = document.getElementById('selectedCategoriesDisplay');
    const categoryInputs = document.getElementById('categoryInputs');
    const selectedCount = document.getElementById('selectedCount');

    // Populate modal with blog details
    var reportModal = document.getElementById('reportBlogModal');
    reportModal.addEventListener('show.bs.modal', function (event) {
        var button = event.relatedTarget; // Button that triggered the modal
        var blogId = button.getAttribute('data-blog-id');
        var authorId = button.getAttribute('data-author-id');
        var authorEmail = button.getAttribute('data-author-email');

        // Update the modal's hidden input fields with the blog details
        document.getElementById('reportBlogId').value = blogId;
        document.getElementById('reportAuthorId').value = authorId;
        document.getElementById('reportAuthorEmail').value = authorEmail;
    });

    function likeBlog(blogId) {
        fetch(`/Blog/Like/${blogId}`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'X-Requested-With': 'XMLHttpRequest',  // Identifies this as an AJAX request
                'RequestVerificationToken': document.querySelector('input[name="__RequestVerificationToken"]').value
            }
        })
            .then(response => {
                if (response.ok) {
                    return response.json();  // Parse the JSON response
                }
            })
            .then(data => {
                // Update the like count in the UI
                if (data && data.likesCount !== undefined) {
                    document.getElementById(`like-count-${blogId}`).textContent = data.likesCount;
                }
            })
            .catch(error => console.error('Error:', error));

        return false;  // Prevent form submission
    }

    // Show/hide dropdown
    searchInput.addEventListener('focus', () => {
        dropdown.style.display = 'block';
    });

    // Close dropdown when clicking outside
    document.addEventListener('click', (e) => {
        if (!e.target.closest('.category-selector')) {
            dropdown.style.display = 'none';
        }
    });

    // Search functionality
    searchInput.addEventListener('input', (e) => {
        const searchTerm = e.target.value.toLowerCase();
        const options = dropdown.querySelectorAll('.category-option');

        options.forEach(option => {
            const name = option.querySelector('.category-name').textContent.toLowerCase();
            option.style.display = name.includes(searchTerm) ? '' : 'none';
        });
    });

    // Category selection
    dropdown.addEventListener('click', (e) => {
        const option = e.target.closest('.category-option');
        if (!option) return;

        const id = option.dataset.id;
        const name = option.dataset.name;

        option.classList.toggle('selected');
        updateSelection(id, name, option.classList.contains('selected'));
        updateCount();
    });

    // Remove tag
    selectedDisplay.addEventListener('click', (e) => {
        if (e.target.classList.contains('remove-tag')) {
            const tag = e.target.closest('.selected-tag');
            const id = tag.dataset.id;

            // Update dropdown selection
            const option = dropdown.querySelector(`.category-option[data-id="${id}"]`);
            if (option) option.classList.remove('selected');

            tag.remove();
            removeInput(id);
            updateCount();
        }
    });

    function updateSelection(id, name, isSelected) {
        if (isSelected) {
            // Add tag
            const tag = document.createElement('span');
            tag.className = 'selected-tag';
            tag.dataset.id = id;
            tag.innerHTML = `${name}<i class="fas fa-times remove-tag"></i>`;
            selectedDisplay.appendChild(tag);

            // Add hidden input
            const input = document.createElement('input');
            input.type = 'hidden';
            input.name = 'categoryIds';
            input.value = id;
            categoryInputs.appendChild(input);
        } else {
            // Remove tag and input
            const tag = selectedDisplay.querySelector(`.selected-tag[data-id="${id}"]`);
            if (tag) tag.remove();
            removeInput(id);
        }
    }

    function removeInput(id) {
        const input = categoryInputs.querySelector(`input[value="${id}"]`);
        if (input) input.remove();
    }

    function updateCount() {
        const count = categoryInputs.querySelectorAll('input').length;
        selectedCount.textContent = `${count} ${count === 1 ? 'category' : 'categories'} selected`;
    }
});
